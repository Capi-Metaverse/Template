using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Fusion;

public class AdvancePresentation : NetworkBehaviour, IMetaEvent
{
   public Presentation presentation;

   public void activate(bool host)
   {
      presentation.OnAdvance();

      if (host)
      {

            //The event is sent to the other users
            RPC_AdvancePress("Advance");
        }
     }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_AdvancePress(string content, RpcInfo info = default)
    {
        Debug.Log("RPC: " + content);
        //Local invoke client
        if (info.IsInvokeLocal)
            Debug.Log("Debug: InvokeLocal");
        else
        activate(false);//Executed on clients
    }
}
