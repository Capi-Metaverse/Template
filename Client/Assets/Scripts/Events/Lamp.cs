using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;


public class Lamp : MonoBehaviour, IMetaEvent
{

    public new Light light;

    
    public void activate(bool host){
        //light is activated
        light.enabled = !light.enabled;
        if (host)
        {
            object[] content = new object[] { this.name}; 
            //The event is sent to the other users
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others }; // You would have to set the Receivers to All in order to receive this event on the local client as well
            PhotonNetwork.RaiseEvent(21, content, raiseEventOptions, SendOptions.SendReliable);
        }
    }   
}
