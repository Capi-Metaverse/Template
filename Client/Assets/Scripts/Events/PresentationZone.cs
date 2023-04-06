using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PresentationZone : MonoBehaviour
{
    //Camera of the presentation
    public Camera cameraObject;

    
    //Detect if it in collider
    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<NetworkObject>())
        {
            CharacterMovementHandler player = other.gameObject.GetComponent<CharacterMovementHandler>();

            //CharacterMovementHandler playerSpawn = player.playerSpawner;

            //playerSpawn.setPresentationCamera(cameraObject);

            MusicManager musicController = GameObject.Find("Manager").GetComponent<MusicManager>();
            musicController.ChangeAudioState();
        }
    }
    //Detect if Exits collider
    private void OnTriggerExit(Collider other) {

        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<NetworkObject>())
        {
            CharacterMovementHandler player = other.gameObject.GetComponent<CharacterMovementHandler>();
           
            //PlayerSpawn playerSpawn =  player.playerSpawner;
            //playerSpawn.setPresentationCamera(null);
        
            MusicManager musicController = GameObject.Find("Manager").GetComponent<MusicManager>();
            musicController.ChangeAudioState();
        }
    }
}
