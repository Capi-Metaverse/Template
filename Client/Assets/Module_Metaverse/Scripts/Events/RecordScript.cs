using RockVR.Video;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordScript : MonoBehaviour, IMetaEvent
{

    GameObject _eventObject;

     [SerializeField] private GameObject camera;
    GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }

    private bool isPlayVideo = false;
    private void Awake()
    {
        Application.runInBackground = true;
        isPlayVideo = false;
    }
    /// <summary>
    /// Start an finish the recording
    /// </summary>
    /// <param name="host"></param>
    public void activate(bool host)
    {
        camera.SetActive(true);
        if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.NOT_START)
        {
            //Start to capture the video
            VideoCaptureCtrl.instance.StartCapture();
        }
        else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.STARTED || VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.PAUSED)
        { 
                //Stop the video
            VideoCaptureCtrl.instance.StopCapture();
            camera.SetActive(false);
        }
    } 
}
