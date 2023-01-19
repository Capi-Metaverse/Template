using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Windows;
using SFB;

public class FileExplorer : MonoBehaviour
{
    public RawImage image;
    private string _path;

    // Open file with filter
    //var extensions = new [] {new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),new ExtensionFilter("Sound Files", "mp3", "wav" ),new ExtensionFilter("All Files", "*" ),};

    // Start is called before the first frame update
    void Start()
    {
        Screen.lockCursor = false;
        //path = EditorUtility.OpenFilePanel("Overwrite with png, txt", "" , "png;txt");
        WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false));
        Screen.lockCursor = true;//Unity and standalone
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*void GetFile()
    {
        if (path != null)
        {
            UpdateImage();
        }
    }
    */
    void UpdateImage()
    {
        Debug.Log("Update image");
        WWW www = new WWW("file://" + _path);
        image.texture = www.texture;
    }
    
    public void WriteResult(string[] paths) {
        if (paths.Length == 0) {
            return;
        }

        _path = "";
        foreach (var p in paths) {
            _path += p + "\n";
            UpdateImage();
        }
    }
    
    public void WriteResult(string path) {
        Debug.Log(path);
        if (path != null)
        {
            _path = path;
            Debug.Log(_path);
            //image.gameObject.SetActive(true);
            UpdateImage();
        }
        else
        {
            Debug.Log("Variable path is null");
        }
    }
}
