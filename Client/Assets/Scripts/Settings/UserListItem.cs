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
    public int numActor { get; set; }

    public void Start()
    {
        gameManager = GameManager.FindInstance();
    }
    public void KickPlayer() { 
        GameManager.RPC_onKick(gameManager.GetRunner(),numActor);  
    }


  
}