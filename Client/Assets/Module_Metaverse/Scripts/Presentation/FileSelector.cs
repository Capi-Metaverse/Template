using System.Collections;
using System;
using System.Linq;
using UnityEngine;
using SFB;
using TMPro;
using UnityEngine.Video;


public class FileSelector: MonoBehaviour
{
    /*---------------------VARIABLES-------------------------*/
    
    public float Size;
    private string _path;//File path
    private GameManager GameManager;

    //UI
    private Presentation Presentation;
    public TMP_Text LoadingPressCanvas;


    //File Upload
    private ImageUpload ImageUpload;
    private VideoUpload VideoUpload;
    private PresentationUpload PresentationUpload;

#if UNITY_WEBGL && !UNITY_EDITOR
        //
        // WebGL
        //
        [DllImport("__Internal")]
        private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);
#endif

    void Start()
    {
        Presentation = GameObject.Find("Presentation").GetComponent<Presentation>();//Getting the press from scene
        GameManager = GameManager.FindInstance();
    }

    /// <summary>
    /// Open panel to choose the file and limiting the extensions to choose
    /// </summary>
    [Obsolete]
    public void OpenFile()
    {
        LoadingPressCanvas.SetText("Loading");
        Screen.lockCursor = false;//Unity and standalone

        var extensions = new[] { new ExtensionFilter("Powerpoint Files", "pptx", "ppt"), new ExtensionFilter("Excel Files", "xlsx", "xlsm", "xlsb"), new ExtensionFilter("Image Files", "png", "jpg", "jpeg"), new ExtensionFilter("Video Files", "mp4", "ogv", "vp8", "webm"), new ExtensionFilter("All Files", "*"), };

        #if UNITY_WEBGL && !UNITY_EDITOR
                UploadFile(gameObject.name, "OnFileUpload", ".pptx", false);
        #else
                WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false));
        #endif
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
            Debug.Log("WriteResultPath: " + _path);
        }

        StartCoroutine(UpdateFile());
    }

    /// <summary>
    /// Transform the path to byte and call Api, eith the extenion and this bytes
    /// </summary>
    /// <returns></returns>
    [System.Obsolete]
    IEnumerator UpdateFile()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
                   UnityWebRequest www = UnityWebRequest.Get(_path);
                    yield return www.SendWebRequest();
                    Debug.Log(www.downloadHandler.text);
                    byte[] bytes = www.downloadHandler.data;
        #else
                WWW www = new WWW("file://" + _path);
                yield return www.isDone;
                Debug.Log("www.data : " + www.data);
                byte[] bytes = www.bytes;
        #endif
        Debug.Log(bytes.Length);

        ClearScreen();

        //Determine file extension
        var fileExtension = _path.Split('.').Last().Trim();
        Debug.Log($"Extension: {fileExtension}");
        switch (fileExtension)
        {

            //Image
            case "jpg":
            case "png":
            case "jpeg":
                Debug.Log("Image");
                ImageUpload = new ImageUpload(Size,Presentation,LoadingPressCanvas);
                ImageUpload.FileUpload(bytes);
                break;

            //Video
            case "mp4":
            case "ogv":
            case "vp8":
            case "webm":
                Debug.Log("Video");
                VideoUpload = new VideoUpload(_path, Presentation, LoadingPressCanvas);
                VideoUpload.FileUpload(bytes, fileExtension);
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
                PresentationUpload = new PresentationUpload(Size, Presentation, GameManager);
                PresentationUpload.FileUpload(bytes, fileExtension);
                break;
            default:
                Debug.Log("Format file not allowed");
                break;
        }
    }

    public void ClearScreen()
    {
        Presentation.sprites.Clear();
        Presentation.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        Presentation.transform.GetChild(0).GetComponent<VideoPlayer>().url = "";
        Presentation.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
    }
}
