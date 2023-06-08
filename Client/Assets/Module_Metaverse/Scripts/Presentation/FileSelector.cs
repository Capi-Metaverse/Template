using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;
using UnityEngine;
using SFB;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine.Video;


public class FileSelector: MonoBehaviour
{
    /*---------------------VARIABLES-------------------------*/
    public Presentation presentation;
    private Compressor compressor = new Compressor();
    private string _path;//File path
    public JObject json;
    UnityWebRequest GetRequest;
    public TMP_Text loadingPressCanvas;//shows when presentation is loading
    public float size;
    object[] content;
    public GameManager gameManager;

    #if UNITY_WEBGL && !UNITY_EDITOR
        //
        // WebGL
        //
        [DllImport("__Internal")]
        private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);
    #endif

    /*---------------------METHODS-------------------------*/
    // Start is called before the first frame update
    void Start()
    {
        presentation = GameObject.Find("Presentation").GetComponent<Presentation>();//Getting the press from scene
        gameManager = GameManager.FindInstance();
    }

    /// <summary>
    /// Open panel to choose the file and limiting the extensions to choose
    /// </summary>
    [Obsolete]
    public void OpenFile()
    {
        loadingPressCanvas.SetText("Loading");
        Screen.lockCursor = false;//Unity and standalone

        var extensions = new[] { new ExtensionFilter("Powerpoint Files", "pptx", "ppt"), new ExtensionFilter("Excel Files", "xlsx", "xlsm", "xlsb"), new ExtensionFilter("Image Files", "png", "jpg", "jpeg"), new ExtensionFilter("Video Files", "mp4", "ogv", "vp8", "webm"), new ExtensionFilter("All Files", "*"), };

        #if UNITY_WEBGL && !UNITY_EDITOR
            UploadFile(gameObject.name, "OnFileUpload", ".pptx", false);
        #else
            //WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false));
        #endif
        Screen.lockCursor = true;//Unity and standalone
    }
}
