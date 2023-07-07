using UnityEngine;

namespace RecorderMetaverse
{
    /// <summary>
    /// Controls the state of the camera used by the recorder
    /// </summary>
    public class RecorderCameraController : MonoBehaviour
    {
        [SerializeField] private GameObject RecorderCamera;

        public void ActivateCamera()
        {
            RecorderCamera.SetActive(true);
        }

        public void DeactivateCamera()
        {
            RecorderCamera.SetActive(false);
        }
    }
}
