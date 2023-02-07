using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PresentationZone : MonoBehaviour
{
    public Camera camera;

    
    //Detect if it in collider
    private void OnTriggerEnter(Collider other) {
        Debug.Log("Contacto");
        if(other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PhotonView>().IsMine)
        {
            Debug.Log(camera);
            Debug.Log(other.gameObject);
            SC_FPSController player = other.gameObject.GetComponent<SC_FPSController>();
            Debug.Log(player);
            PlayerSpawn playerSpawn = player.playerSpawner;
            Debug.Log(player.playerSpawner);
            playerSpawn.setPresentationCamera(camera);
        
            
        }
    }
    //Detect if Exits collider
    private void OnTriggerExit(Collider other) {
        Debug.Log("Salida");

         if(other.gameObject.CompareTag("Player"))
        {
            SC_FPSController player = other.gameObject.GetComponent<SC_FPSController>();
            Debug.Log(player);
            PlayerSpawn playerSpawn =  player.playerSpawner;
            playerSpawn.setPresentationCamera(null);
        
            
        }
        
    }
}
