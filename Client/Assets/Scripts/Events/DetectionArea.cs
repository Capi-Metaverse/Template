using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DetectionArea : MonoBehaviour
{
    private CharacterController playerController;
    public UnityEvent detectionEvent;
    public GameObject MyPJ;

    
    private void Update()
    {
        //viewplayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PhotonView>().IsMine;
        GameObject[] viewplayer = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in viewplayer) 
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                MyPJ=player;
                playerController=player.GetComponent<CharacterController>();
                break;  
            }
        }
    }
    //Detect if it in collider
    private void OnTriggerEnter(Collider other) {
        Debug.Log("Contacto");
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("La colision fue con el Player");
            detectionEvent.Invoke();
        }
    }
    //Detect if Exits collider
    private void OnTriggerExit(Collider other) {
        Debug.Log("Salida");
        
    }
    //Change the location
    public void Respawn(Transform pointToSpawn)
    {
        playerController.enabled=false;
        MyPJ.transform.position = pointToSpawn.position;
        playerController.enabled=true;
    }
}
