using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Fusion;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserListItem : NetworkBehaviour
{
    GameManager gameManager;
    public int numActor;

    public void Start()
    {
        gameManager = GameManager.FindInstance();
    }
    public void KickPlayer() { 
        RPC_onKick(numActor);  
    }


    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_onKick(int numActor)
    {

        Debug.Log(numActor);
       
        var juan = gameManager.GetCurrentPlayer().GetComponent<NetworkPlayer>().ActorID;
        Debug.Log(juan);
        if (numActor == juan)
        {
            gameManager.Disconnect();

            SceneManager.LoadSceneAsync("Lobby");


            Destroy(this.gameObject);

        }

    }
}