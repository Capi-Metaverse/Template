using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Fusion;

public class AdvancePresentation : NetworkBehaviour, IMetaEvent
{
   public Presentation presentation;
    GameObject eventObject;

    public void activate(bool host)
    {
        GameManager.RPC_AdvancePress(GameManager.FindInstance().GetRunner());
    }
}
