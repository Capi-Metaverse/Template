using RockVR.Video;
using UnityEngine;

public class RecordScript : MonoBehaviour, IMetaEvent
{

    GameObject _eventObject;

    [SerializeField] private RecorderCameraController _recorderCameraController;
    [SerializeField] private GameObject VideoRecorder;
    GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }

    private void Awake()
    {
        Application.runInBackground = true;
    }
    /// <summary>
    /// Start an finish the recording
    /// </summary>
    /// <param name="host"></param>
    public void activate(bool host)
    {
        VideoRecorder.SetActive(true);
        if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.NOT_START)
        {
            _recorderCameraController.ActivateCamera();
            //Start to capture the video
            VideoCaptureCtrl.instance.StartCapture();
        }
        else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.STARTED || VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.PAUSED)
        { 
                //Stop the video
            VideoCaptureCtrl.instance.StopCapture();
            VideoRecorder.SetActive(false);
            _recorderCameraController.DeactivateCamera();
        }
    } 
}
