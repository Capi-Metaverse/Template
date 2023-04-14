using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PresentationZone : MonoBehaviour
{
    //Camera of the presentation
    public Camera cameraObject;

    CharacterInputHandler playerInputs;


    //Detect if it in collider
    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.Equals(GameManager.FindInstance().GetCurrentPlayer().gameObject))
        {


            playerInputs = GameManager.FindInstance().GetCurrentPlayer().GetComponent<CharacterInputHandler>();
            playerInputs.setPresentationCamera(cameraObject);

            MusicManager musicController = GameObject.Find("Manager").GetComponent<MusicManager>();
            musicController.ChangeAudioState();
        }
    }
    //Detect if Exits collider
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
