using RockVR.Video;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseRecScript : MonoBehaviour, IMetaEvent
{
    private bool isPlayVideo = false;

    GameObject _eventObject;
    public GameObject eventObject { get => _eventObject; set => _eventObject = value; }

    private void Awake()
    {
        Application.runInBackground = true;
        isPlayVideo = false;
    }
    /// <summary>
    /// To Pause the recording
    /// </summary>
    /// <param name="host"></param>
    public void activate(bool host)
    {
        //Set Pause
        if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.STARTED)
        {
            VideoCaptureCtrl.instance.ToggleCapture();

        }
        //Set UnPause
        else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.PAUSED)
        {

            VideoCaptureCtrl.instance.ToggleCapture();

        }
    }
}
