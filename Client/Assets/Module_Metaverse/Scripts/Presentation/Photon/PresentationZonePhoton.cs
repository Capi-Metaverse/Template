using UnityEngine;

public class PresentationZonePhoton : MonoBehaviour
{
    //Camera of the presentation
    public Camera cameraObject;

    CharacterInputHandler playerInputs;

    /// <summary>
    /// Detect if it in collider, activate the Ui Press K and mute the music
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.Equals(GameManager.FindInstance().GetCurrentPlayer().gameObject)) //TODO: Modify getcurrentplayer so that it is compatible with single player
        {
            playerInputs = GameManager.FindInstance().GetCurrentPlayer().GetComponent<CharacterInputHandler>();
            playerInputs.setPresentationCamera(cameraObject);

            MusicManager musicController = GameObject.Find("Manager").GetComponent<MusicManager>();
            musicController.ChangeAudioState();
        }  
    }
    /// <summary>
    /// Detect if Exits collider, desactivate the UI Press K and active the music
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other) {

        if (other.gameObject.Equals(GameManager.FindInstance().GetCurrentPlayer().gameObject))
        {
            playerInputs = GameManager.FindInstance().GetCurrentPlayer().GetComponent<CharacterInputHandler>();
            playerInputs.setPresentationCamera(null);

            MusicManager musicController = GameObject.Find("Manager").GetComponent<MusicManager>();
            musicController.ChangeAudioState();
        }
    }
}
