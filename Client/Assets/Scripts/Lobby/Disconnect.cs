using System.Collections;
 using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class Disconnect : MonoBehaviourPunCallbacks
{
    public GameObject Settings;
    public GameObject Pausa;

    //Voice Chat
    private AudioController voiceChat;
    private InputManager inputManager;
    private MusicManager musicController;

    private PlayerSpawn playerManager;
    PlayerList playerList;
    void Start()
    {
      voiceChat = GameObject.Find("VoiceManager").GetComponent<AudioController>();
      playerManager = GameObject.Find("PlayerSpawner").GetComponent<PlayerSpawn>();  
      
    }

    public void OnClickDisconnect()
    {   
        PhotonNetwork.Disconnect();
       
        voiceChat.OnApplicationQuit();
        Application.Quit();
    }

    public void OnClickSettings()
    {   
      Pausa.SetActive(false);
      Settings.SetActive(true);
      //If the user is master  it will search the player list
      if (PhotonNetwork.IsMasterClient)
        {
          playerList = GameObject.Find("TabPlayer").GetComponent<PlayerList>();
          playerList.playerList();
        } 
      
      
    }

    public void OnClickBackToPause()
    {   
      Pausa.SetActive(true);
      Settings.SetActive(false);
    }

      public void OnClickReturnLobby()
    {
      SceneManagerScript  sceneManager=GameObject.Find("SceneManager").GetComponent<SceneManagerScript>();
      sceneManager.onNewMap = false;
      PhotonNetwork.LeaveRoom();
      
    }

    public void OnClickReturnGame()
    {
      playerManager.setJuego();
    }

}
            