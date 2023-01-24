using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class BackPresentation : MonoBehaviour, IMetaEvent
{

   public Presentation presentation;

     public void activate(bool host){

        presentation.OnReturn();

        if (host){

        object[] content = new object[] { "Back"}; 
   
    

        //Se envía el evento a los demás usuarios
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(23, content, raiseEventOptions, SendOptions.SendReliable);

        }

     }
}
