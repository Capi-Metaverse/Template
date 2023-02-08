using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DetectionArea : MonoBehaviour
{
    //Spawn point to the user
    public Transform[] spawnPoints;


    //Detect if it in collider
    private void OnTriggerEnter(Collider other) {
      
     
      //If the user is the one that provokes the collision
        if(other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PhotonView>().IsMine)
        {
            int randomNumber = Random.Range(0, spawnPoints.Length);

    
             
            CharacterController playerController = other.gameObject.GetComponent<CharacterController>();
            
            //Deactivate player controller
            playerController.enabled=false;
           

            //Move the character

                    other.gameObject.transform.position = spawnPoints[randomNumber].position;

            //Reactivate player controller2
            playerController.enabled=true;
      
           
        }
    }
    //Detect if Exits collider
    private void OnTriggerExit(Collider other) {
        Debug.Log("Salida");
        
    }
    
}
