using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Windows;
using SFB;
using UnityEngine.Networking;
using System;
using System.Text;
using Newtonsoft.Json.Linq;

using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;


public class FileExplorer : MonoBehaviour
{
    public Presentation presentation;
    private string _path;
    JObject json;

    // Open file with filter
    //var extensions = new [] {new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),new ExtensionFilter("Sound Files", "mp3", "wav" ),new ExtensionFilter("All Files", "*" ),};

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        presentation=GameObject.Find("Presentation").GetComponent<Presentation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Obsolete]
    public void OpenFile()
    {
        
        Screen.lockCursor = false;//Unity and standalone
        //path = EditorUtility.OpenFilePanel("Overwrite with png, txt", "" , "png;txt");

        //Open panel to choose the file and limiting the extensions to choose
        var extensions = new [] {new ExtensionFilter("Powerpoint Files", "pptx", "ppt"),new ExtensionFilter("Excel Files", "xlsx", "xlsm","xlsb"),new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),new ExtensionFilter("Video Files", "mp4","ogv","vp8","webm" ),new ExtensionFilter("All Files", "*" ),};
        WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false));
        Screen.lockCursor = true;//Unity and standalone
    }
    //Function to make a Get request
    public IEnumerator GetRequestFunc()
    {
        foreach (var archiv in json["Files"]) 
        {
            Debug.Log(archiv["Url"].ToString());
            //Get Request
            UnityWebRequest GetRequest = UnityWebRequest.Get(archiv["Url"].ToString());
                yield return GetRequest.SendWebRequest();

                switch (GetRequest.result)
                {

                        case UnityWebRequest.Result.ConnectionError:

                        case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError( "Error: " + GetRequest.error);
                        break;

                        case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError("HTTP Error: " + GetRequest.error);
                        break;

                        case UnityWebRequest.Result.Success:
                            
                            Texture2D textu = new Texture2D(1, 1);
                            Debug.Log(GetRequest.downloadHandler.data);
                            //transform data into texture
                            textu.LoadImage(GetRequest.downloadHandler.data);
                            //transform texture into Sprite
                            var nuevoSprite = Sprite.Create(textu, new Rect(0.0f, 0.0f, textu.width, textu.height), new Vector2(0.57f, 0.5f)); 
                            presentation.sprites.Add(nuevoSprite);
                    break;
                }
                     
        }
        //function in presentation to check if the list is full
        presentation.OnDirect();     
    }

    [System.Obsolete]
    IEnumerator UpdateImage()
    {
        WWW www = new WWW("file://" + _path);
        yield return www.isDone;
        byte[] bytes = www.bytes;
        Debug.Log(bytes.Length);
        //Convert data un Base64
        string s = Convert.ToBase64String(bytes);

        //JSON needed to make a post
        var file = new JObject
        {
            ["Name"] = "file.pptx",
            ["Data"] = s
        };
    

        var parameters = new JArray
        {
        new JObject
            {
                ["Name"] = "file",
                ["FileValue"] = file
            },
        new JObject
            {
                ["Name"] = "StoreFile",
                ["Value"] = true
            }
        };

        var jsonObject = new JObject
        {
        ["Parameters"] = parameters
        };

        string jsonString = jsonObject.ToString();
        Debug.Log(jsonString);

        //POST to ConvertApi
        UnityWebRequest request = new UnityWebRequest("https://v2.convertapi.com/convert/pptx/to/png?Secret=T0TzTuNoju79aVtJ", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
                    
        json = JObject.Parse(request.downloadHandler.text);

        //We build the content item
                        
        object[] content = new object[] { request.downloadHandler.text}; 
    
        //We send the content to the other users
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(22, content, raiseEventOptions, SendOptions.SendReliable);

        //Si es nula
        if(presentation.current >= 1) presentation.current=0;
        if(presentation.sprites != null) presentation.sprites.Clear();
        StartCoroutine(GetRequestFunc());              
    }
 
    [System.Obsolete]
    public void WriteResult(string[] paths) 
    {
        if (paths.Length == 0) 
        {
            return;
        }
        _path = "";
        foreach (var p in paths) 
        {
            _path += p + "\n";
            StartCoroutine(UpdateImage());
        }
    }

    [System.Obsolete]
    public void WriteResult(string path) 
    {
        Debug.Log(path);
        if (path != null)
        {
            _path = path;
            Debug.Log(_path);
            //image.gameObject.SetActive(true);
            StartCoroutine(UpdateImage());
        }
        else
        {
            Debug.Log("Variable path is null");
        }
    }

    public  void downloadImages(string file)
    {
        if(presentation.current >= 1) presentation.current=0;
        if(presentation.sprites != null) presentation.sprites.Clear();
        
        Debug.Log("Entr2o");
        json = JObject.Parse(file);
        Debug.Log("Entro");
        StartCoroutine(GetRequestFunc());
    }
}

