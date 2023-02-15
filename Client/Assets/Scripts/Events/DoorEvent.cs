using System.Collections;
using System.Collections.Generic;
 using System;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class DoorEvent : MonoBehaviourPunCallbacks, IMetaEvent
{
    
    public string map;
    SceneManagerScript sceneManager;

   


     public void activate(bool host){

      sceneManager=GameObject.Find("SceneManager").GetComponent<SceneManagerScript>();
      sceneManager.currentMapNumber = Convert.ToInt32(map) ;
        PhotonNetwork.LeaveRoom();
      
     

      
       
       
    }   

 
}
