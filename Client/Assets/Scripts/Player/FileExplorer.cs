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

public class FileExplorer : MonoBehaviour
{
    public Presentation presentation;
    public RawImage image;
    private string _path;
    List<string> urls = new List<string>();
    public List<Texture2D> Img = new List<Texture2D>();
    
    

    // Open file with filter
    //var extensions = new [] {new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),new ExtensionFilter("Sound Files", "mp3", "wav" ),new ExtensionFilter("All Files", "*" ),};

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        Screen.lockCursor = false;
        //path = EditorUtility.OpenFilePanel("Overwrite with png, txt", "" , "png;txt");
        WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false));
        Screen.lockCursor = true;//Unity and standalone
        presentation=GameObject.Find("Presentation").GetComponent<Presentation>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [System.Obsolete]
    IEnumerator UpdateImage()
    {
        Debug.Log("Update image");
        WWW www = new WWW("file://" + _path);
        yield return www.isDone;
        byte[] bytes = www.bytes;
        Debug.Log(bytes.Length);
        //image.texture = www.texture;
        string s = Convert.ToBase64String(bytes);

        
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


    

                    UnityWebRequest request = new UnityWebRequest("https://v2.convertapi.com/convert/pptx/to/png?Secret=T0TzTuNoju79aVtJ", "POST");
                    byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);
                    request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
                    request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
                    request.SetRequestHeader("Content-Type", "application/json");

                    yield return request.SendWebRequest();
                 
                    JObject json = JObject.Parse(request.downloadHandler.text);

                 
                    foreach (var archiv in json["Files"]) {
                        
                        urls.Add(archiv["Url"].ToString());
                        Debug.Log(archiv["Url"].ToString());

                    UnityWebRequest GetRequest = UnityWebRequest.Get(archiv["Url"].ToString());

                        yield return GetRequest.SendWebRequest();

                      switch (GetRequest.result){
                        case UnityWebRequest.Result.ConnectionError:
                        case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError( "Error: " + GetRequest.error);
                        break;
                        case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + GetRequest.error);
                        break;
                        case UnityWebRequest.Result.Success:
                    Debug.Log("llego aqui Successsssss");
                            
                            Texture2D textu = new Texture2D(1, 1);
                            Debug.Log(GetRequest.downloadHandler.data);
                            textu.LoadImage(GetRequest.downloadHandler.data);
                        
                            Img.Add(textu);
                            var nuevoSprite = Sprite.Create(textu, new Rect(0.0f, 0.0f, textu.width, textu.height), new Vector2(0.5f, 0.5f)); 
                            presentation.sprites.Add(nuevoSprite);
                    break;
                      }
                     
                    } 
                         
                    
    }
 
    [System.Obsolete]
    public void WriteResult(string[] paths) {
        if (paths.Length == 0) {
            return;
        }

        _path = "";
        foreach (var p in paths) {
            _path += p + "\n";
            StartCoroutine(UpdateImage());
        }
    }

    [System.Obsolete]
    public void WriteResult(string path) {
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
}
