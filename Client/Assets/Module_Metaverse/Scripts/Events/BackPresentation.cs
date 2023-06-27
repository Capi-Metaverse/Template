using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using ExitGames.Client.Photon;
using Photon.Realtime;
/// <summary>
/// Event to activate the slice back in the presentation
/// </summary>
public class BackPresentation : NetworkBehaviour, IMetaEvent
{
   public Presentation presentation;
    GameObject _eventObject;
    PhotonManager photonManager;
    GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }

    public void activate(bool host)
    {
        photonManager = PhotonManager.FindInstance();
        RPCManager.RPC_BackPress(photonManager.Runner);
    }
}
