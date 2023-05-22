using SFB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRecordPath : MonoBehaviour, IMetaEvent
{

    GameObject _eventObject;
    public GameObject eventObject { get => _eventObject; set => _eventObject = value; }

    string _path;
    public void activate(bool host)
    {
        OpenFile();

        
    }


    public void OpenFile()
    {
        Cursor.lockState = CursorLockMode.None;//Unity and standalone

        //Open panel to choose the file and limiting the extensions to choose
        var extensions = new[] { new ExtensionFilter() };


        WriteResult(StandaloneFileBrowser.OpenFilePanel("Select Folder", "", extensions, false));

        Cursor.lockState = CursorLockMode.Locked;//Unity and standalone
    }

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

   
    }


}
