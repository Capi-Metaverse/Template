using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Fusion;
/// <summary>
/// Event to activate the slice advance in the presentation
/// </summary>
public class AdvancePresentation : NetworkBehaviour, IMetaEvent
{
   public Presentation presentation;

    GameObject _eventObject;
    GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }

    public void activate(bool host)
    {
        GameManager.RPC_AdvancePress(GameManager.FindInstance().GetRunner());
    }
}
