using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UploadPPTX : MonoBehaviour, IMetaEvent
{
    public GameObject ChooseFile;
    public FileSelector fileSelector;
    bool activado=false;
    public TMP_Text loadingPressCanvas;
    GameObject _eventObject;
    GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }

    [System.Obsolete]
    public void activate(bool host)
    {
        ChooseFile.SetActive(!activado);
        loadingPressCanvas.enabled = true;
        loadingPressCanvas.SetText("Loading");
        fileSelector.OpenFile();   
    }  
}
