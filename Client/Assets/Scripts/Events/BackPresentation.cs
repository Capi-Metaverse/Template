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

      presentation.OnReturn();

      if (host)
      {
            //The event is sent to the other users
            RPC_BackPress("Back");
      }
   }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_BackPress(string content, RpcInfo info = default)
    {

        Debug.Log("RPC: " + content);
        if (info.IsInvokeLocal)
            Debug.Log("Debug: InvokeLocal");
        else    
        activate(false);//All clients except who invoked it
    }
}
