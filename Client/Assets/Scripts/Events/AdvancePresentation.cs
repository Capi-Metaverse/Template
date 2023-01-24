using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class AdvancePresentation : MonoBehaviour, IMetaEvent
{
    public Presentation presentation;

     public void activate(bool host){

        presentation.OnAdvance();

        if (host){

        object[] content = new object[] { "Advance"}; 
   
    

        //Se envía el evento a los demás usuarios
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(23, content, raiseEventOptions, SendOptions.SendReliable);

        }

     }
    
}
