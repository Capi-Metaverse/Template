using RockVR.Video;
using UnityEngine;

namespace RecorderMetaverse
{
    public class PauseRecScript : MonoBehaviour, IMetaEvent
    {

        GameObject _eventObject;
        public GameObject eventObject { get => _eventObject; set => _eventObject = value; }

        private void Awake()
        {
            Application.runInBackground = true;
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
}
