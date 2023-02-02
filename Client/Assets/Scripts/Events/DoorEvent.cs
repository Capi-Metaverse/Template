using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class DoorEvent : MonoBehaviour, IMetaEvent
{
    public Transform spawnPoint;

     public void activate(bool host){

       GameObject[] viewplayer = GameObject.FindGameObjectsWithTag("Player");

         
        foreach (GameObject player in viewplayer) 
        {
            Debug.Log(player);
            Debug.Log(player.transform);
            Debug.Log(player.transform.position);
            if (player.GetComponent<PhotonView>().IsMine)
            {
               CharacterController playerController=player.GetComponent<CharacterController>();
               playerController.enabled=false;
                player.transform.position = spawnPoint.position;
                playerController.enabled=true;
                break;  
            }
        }
       //SC_FPSController playerController=user.GetComponent<SC_FPSController>();


       
        
    }   

 
}
