using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Fusion;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserListItem : MonoBehaviour
{
    GameManager gameManager;
    public int numActor;

    public void Start()
    {
        gameManager = GameManager.FindInstance();
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void KickPlayer()
    {
        int[] myNum = { numActor };
        var juan = gameManager.GetCurrentPlayer().GetComponent<NetworkPlayer>().ActorID;

        if (numActor == juan)
        {
            gameManager.Disconnect();

            SceneManager.LoadSceneAsync("Lobby");


            Destroy(this.gameObject);

        }


       
    }
}