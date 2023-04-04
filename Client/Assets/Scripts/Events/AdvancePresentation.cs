using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Fusion;

public class AdvancePresentation : NetworkBehaviour, IMetaEvent
{
   public Presentation presentation;
    object[] content;

   public void activate(bool host)
   {

      presentation.OnAdvance();

      if (host)
      {

      object[] content = new object[] { "Advance"};
            //The event is sent to the other users
            //RPC_AdvancePress(content);
        }
     }

    /*[Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_AdvancePress(object[] content, RpcInfo info = default)
    {
        Debug.Log("RPC: " + content);
        this.content = content;
    }
    */
}
