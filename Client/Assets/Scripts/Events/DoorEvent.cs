using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class DoorEvent : MonoBehaviour, IMetaEvent
{
    
    public string map;
    public Disconnect disconnectScript;


     public void activate(bool host){

        Debug.Log("AQUI");
        disconnectScript.changeRoom(map);
       
    }   

 
}
