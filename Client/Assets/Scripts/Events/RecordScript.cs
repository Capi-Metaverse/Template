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
        else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.STARTED || VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.PAUSED)
        {
           
                //Stop the video
                VideoCaptureCtrl.instance.StopCapture();
          
          
               
            
        }
     

    }

  
}
