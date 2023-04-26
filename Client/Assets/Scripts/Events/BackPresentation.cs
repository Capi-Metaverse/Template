using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class BackPresentation : NetworkBehaviour, IMetaEvent
{
   public Presentation presentation;

    public void activate(bool host)
    {
        GameManager.RPC_BackPress(GameManager.FindInstance().GetRunner());
    }
}
