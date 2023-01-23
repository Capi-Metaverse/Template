using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;


public class Lamp : MonoBehaviour, IMetaEvent
{

    public Light light;

    
    public void activate(){
        //Se activa la luz
        light.enabled = !light.enabled;
        object[] content = new object[] { this.name}; 
   
    

        //Se envía el evento a los demás usuarios
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(21, content, raiseEventOptions, SendOptions.SendReliable);

    }

    
}
