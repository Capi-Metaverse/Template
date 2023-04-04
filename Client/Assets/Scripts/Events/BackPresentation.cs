using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class BackPresentation : NetworkBehaviour, IMetaEvent
{
   public Presentation presentation;
   object[] content;

   public void activate(bool host)
   {

      presentation.OnReturn();

      if (host)
      {

      object[] content = new object[] { "Back"};
            //The event is sent to the other users
            //RPC_BackPress(content);
      }
   }

    /*[Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_BackPress(object[] content, RpcInfo info = default)
    {
        Debug.Log("RPC: " + content);
        this.content = content;
    }
    */
}
