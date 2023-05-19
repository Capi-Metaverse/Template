using RockVR.Video;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordScript : MonoBehaviour, IMetaEvent
{

    GameObject _eventObject;
    GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }

    private bool isPlayVideo = false;
    private void Awake()
    {
        Application.runInBackground = true;
        isPlayVideo = false;
    }

    public void activate(bool host)
    {
        if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.NOT_START)
        {
            //Start to capture the video
            VideoCaptureCtrl.instance.StartCapture();
          
        }
        else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.STARTED)
        {
           
                //Stop the video
                VideoCaptureCtrl.instance.StopCapture();
          
          
                //Pause
                //VideoCaptureCtrl.instance.ToggleCapture();
            
        }
        /*
        else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.PAUSED)
        {
            if (GUI.Button(new Rect(10, Screen.height - 60, 150, 50), "Stop Capture"))
            {
                VideoCaptureCtrl.instance.StopCapture();
            }
            if (GUI.Button(new Rect(180, Screen.height - 60, 150, 50), "Continue Capture"))
            {
                VideoCaptureCtrl.instance.ToggleCapture();
            }
        }
        */
        else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.STOPPED)
        {
            if (GUI.Button(new Rect(10, Screen.height - 60, 150, 50), "Processing"))
            {
                // Waiting processing end.
            }
        }

    }

  
}
