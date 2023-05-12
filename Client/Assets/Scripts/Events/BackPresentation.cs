using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class BackPresentation : NetworkBehaviour, IMetaEvent
{
   public Presentation presentation;
    GameObject _eventObject;
    GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }

    public void activate(bool host)
    {
        GameManager.RPC_BackPress(GameManager.FindInstance().GetRunner());
    }
}
