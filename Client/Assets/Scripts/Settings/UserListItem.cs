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
        
        var juan = gameManager.GetCurrentPlayer().GetComponent<NetworkPlayer>().ActorID;
        Debug.Log(juan);
        Debug.Log(numActor);

            if (numActor == juan)
            {
                gameManager.Disconnect();

                SceneManager.LoadSceneAsync("Lobby");


                Destroy(this.gameObject);

        }
        else
        {
            Debug.Log("ERORRRRRRRRR");
        }

        

       
    }
}