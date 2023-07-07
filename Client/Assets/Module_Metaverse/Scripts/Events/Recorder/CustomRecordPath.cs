using RockVR.Video;
using SFB;
using UnityEngine;

namespace RecorderMetaverse
{
    public class CustomRecordPath : MonoBehaviour, IMetaEvent
    {

        GameObject _eventObject;
        public GameObject eventObject { get => _eventObject; set => _eventObject = value; }

        [SerializeField] private VideoCapture videoCapture;

        string _path;
        public void activate(bool host)
        {
            OpenFile();
        }
        /// <summary>
        /// Open panel to select the directory 
        /// </summary>
        public void OpenFile()
        {
            Cursor.lockState = CursorLockMode.None;//Unity and standalone

            //Open panel to choose the file and limiting the extensions to choose
            var extensions = new[] { new ExtensionFilter() };


            WriteResult(StandaloneFileBrowser.OpenFolderPanel("Custom directory", "", false));


            Cursor.lockState = CursorLockMode.Locked;//Unity and standalone
        }
        /// <summary>
        /// To change the path to save the recording
        /// </summary>
        /// <param name="paths"></param>
        public void WriteResult(string[] paths)
        {
            if (paths.Length == 0)
            {
                return;
            }
            Debug.Log("CustomPath: " + paths[0]);
            _path = paths[0];

            videoCapture.customPath = true;
            videoCapture.customPathFolder = _path;
        }
    }
}
