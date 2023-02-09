using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Windows;
using SFB;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.Video;

public class FileExplorer : MonoBehaviour
{
    /*---------------------VARIABLES-------------------------*/
    public Presentation presentation;
    private Compressor compressor = new Compressor();
    private string _path;//File path
    JObject json;
    UnityWebRequest GetRequest;
    public TMP_Text  loadingPressCanvas;//shows when presentation is loading

    // Open file with filter
    //var extensions = new [] {new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),new ExtensionFilter("Sound Files", "mp3", "wav" ),new ExtensionFilter("All Files", "*" ),};

    /*---------------------METHODS-------------------------*/
    // Start is called before the first frame update
    void Start()
    {
        presentation=GameObject.Find("Presentation").GetComponent<Presentation>();//Getting the press from scene
    }

    [Obsolete]
    public void OpenFile()
    {
        loadingPressCanvas.SetText("Loading");
        Screen.lockCursor = false;//Unity and standalone
        //path = EditorUtility.OpenFilePanel("Overwrite with png, txt", "" , "png;txt");

        //Open panel to choose the file and limiting the extensions to choose
        var extensions = new [] {new ExtensionFilter("Powerpoint Files", "pptx", "ppt"),new ExtensionFilter("Excel Files", "xlsx", "xlsm","xlsb"),new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),new ExtensionFilter("Video Files", "mp4","ogv","vp8","webm" ),new ExtensionFilter("All Files", "*" ),};
        WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false));
        Screen.lockCursor = true;//Unity and standalone
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
            StartCoroutine(UpdateFile());
        }
    }
    
    [System.Obsolete]
    IEnumerator UpdateFile()
    {
        WWW www = new WWW("file://" + _path);
        yield return www.isDone;
        byte[] bytes = www.bytes;
        Debug.Log(bytes.Length);

        //Determine file extension
        var fileExtension = _path.Split('.').Last().Trim();
        Debug.Log($"Extension: {fileExtension}");

        switch (fileExtension){

            //Image
            case "jpg":
            case "png":
            case "jpeg":
                Debug.Log("Image");
                ImageUpload(bytes);
                break;

            //Video
            case "mp4":
            case "ogv":
            case "vp8":
            case "webm":
                Debug.Log("Video");
                VideoUpload(bytes,fileExtension);
                break;

            //Excel
            case "xlsx":
            case "xlsm":
            case "xlsb":
                break;
            
            // Powerpoint
            case "pptx":
            case "ppt":
                Debug.Log("Presentation");
                StartCoroutine(PresentationUpload(bytes,fileExtension,$"https://v2.convertapi.com/convert/{fileExtension}/to/png?Secret=T0TzTuNoju79aVtJ"));
                break;
            default:
                Debug.Log("Format file not allowed");
                break;
        }            
    }

    //Upload the image file and share it with other users
    void ImageUpload(byte[] bytes){
        //We build the content item            
        object[] content = new object[] { bytes}; 
    
        //We send the content to the other users
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(25, content, raiseEventOptions, SendOptions.SendReliable);

        SetImage(bytes);
    }
    public void SetImage(byte[] bytes){
        //Clear videoScreen
        presentation.sprites.Clear();
        presentation.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        presentation.transform.GetChild(0).GetComponent<VideoPlayer>().url="";

        //Create Texture
        Texture2D textu = new Texture2D(1, 1);

        //transform data into texture
        textu.LoadImage(bytes);

        //transform texture into Sprite
        loadingPressCanvas.enabled = false;
        var nuevoSprite = Sprite.Create(textu, new Rect(0.0f, 0.0f, textu.width, textu.height), new Vector2(0.57f, 0.5f)); 
        presentation.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = nuevoSprite;
    }

    //Upload the image file and share it with other users
    void VideoUpload(byte[] bytes,string fileExtension){
        string filename = _path.Split("\\").Last().Split('.')[0].Trim();


        //Online Video, Will be implemented in the future.
        //We build the content item
        // byte[] Compressed = compressor.Compress(bytes);
        // object[] content = new object[] {filename, fileExtension,Compressed};
    
        // //We send the content to the other users
        // RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        // PhotonNetwork.RaiseEvent(24, content, raiseEventOptions, SendOptions.SendReliable);

        SetVideo(filename,fileExtension,bytes);
    }

    public void SetVideo(string filename, string fileExtension, byte[] bytes){
        
        //Prepare videoScreen
        presentation.sprites.Clear();
        presentation.transform.GetChild(0).GetComponent<VideoPlayer>().url="";
        presentation.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;

        //Create VideoFile
        Debug.Log(_path);
        string PostPath = Application.persistentDataPath + $"/{filename}.{fileExtension}";
        if (!System.IO.File.Exists(PostPath))
            System.IO.File.WriteAllBytes(PostPath, bytes);
        
        //Set VideoClip
        loadingPressCanvas.enabled = false;
        presentation.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        VideoPlayer videoPlayer = presentation.transform.GetChild(0).GetComponent<VideoPlayer>();
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = PostPath;

    }

    //Upload the presentation file to the convert api
    IEnumerator PresentationUpload(byte[] bytes,string fileExtension, string PostPath){

        //Clear videoScreen and previous sprite
        presentation.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        presentation.transform.GetChild(0).GetComponent<VideoPlayer>().url="";
        presentation.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;

        //Convert data un Base64
        string s = Convert.ToBase64String(bytes);

        //JSON needed to make a post
        var file = new JObject
        {
            ["Name"] = $"file.{fileExtension}",
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
        UnityWebRequest request = new UnityWebRequest(PostPath, "POST");
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
 
    //Function to make a Get request
    public IEnumerator GetRequestFunc()
    {
        foreach (var archiv in json["Files"]) 
        {
            Debug.Log(archiv["Url"].ToString());
            //Get Request
            UnityWebRequest GetRequest = UnityWebRequestTexture.GetTexture(archiv["Url"].ToString());
            yield return GetRequest.SendWebRequest();
            
                
                switch (GetRequest.result)
                {
                    //Error cases
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError( "Error: " + GetRequest.error);
                    break;
                    case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + GetRequest.error);
                    break;
                    //Success case
                    case UnityWebRequest.Result.Success:
                        //We create a texture2D with the response data
                        Texture2D textu = ((DownloadHandlerTexture)GetRequest.downloadHandler).texture;
                        var nuevoSprite = Sprite.Create(textu, new Rect(0.0f, 0.0f, textu.width, textu.height), new Vector2(0.57f, 0.5f)); 
                        presentation.sprites.Add(nuevoSprite);
                    break;
                }
            //yield return new WaitForSeconds((float)0.3);        
        }

        //function in presentation to check if the list is full
        presentation.OnDirect();     
    }

    public  void downloadImages(string file)
    {
        if(presentation.current >= 1) presentation.current=0;
        if(presentation.sprites != null) presentation.sprites.Clear();
        
        loadingPressCanvas.enabled = false;
        json = JObject.Parse(file);
        Debug.Log("Entro");
        StartCoroutine(GetRequestFunc());
    }
}

