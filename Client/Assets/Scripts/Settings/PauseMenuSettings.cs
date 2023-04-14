using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
//using ExitGames.Client.Photon;

public class PauseMenuSettings : MonoBehaviour
{
    GameManager gameManager;
    public GameObject Settings;
    public GameObject Pause;
    [SerializeField] private TMP_Text roomName;

    //Voice Chat
    /*  private AudioController voiceChat;
      private InputManager inputManager;
      private MusicManager musicController;*/

    /*   private PlayerSpawn playerManager;*/
     PlayerList playerList;
    /* void Start()
     {
       voiceChat = GameObject.Find("VoiceManager").GetComponent<AudioController>();
       playerManager = GameObject.Find("PlayerSpawner").GetComponent<PlayerSpawn>();  

     }*/

    private void Start()
    {
        
        Settings = GameObject.Find("Menus").transform.GetChild(0).gameObject;
        Pause = GameObject.Find("Menus").transform.GetChild(1).gameObject;
        gameManager = GameManager.FindInstance();
        roomName.text = gameManager.GetMapName();
    }

    public void OnClickDisconnect()
    {
        gameManager.Disconnect();
        SceneManager.LoadSceneAsync("1.Start");


    }

    public void OnClickSettings()
    {
        Pause.SetActive(false);
        Settings.SetActive(true);
        //If the user is master  it will search the player list
         if (gameManager.GetUserRole() == UserRole.Admin)
            {
              playerList = GameObject.Find("TabPlayer").GetComponent<PlayerList>();
              playerList.ListPlayers();
    } 


    }

    public void OnClickBackToPause()
    {
        Pause.SetActive(true);
        Settings.SetActive(false);
    }

    public void OnClickReturnLobby()
    {
        gameManager.Disconnect();
       
        SceneManager.LoadSceneAsync("Lobby");
       

    }

    /* public void OnClickReturnGame()
     {
       playerManager.setJuego();
     }*/

}
