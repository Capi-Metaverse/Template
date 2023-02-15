using System.Collections;
 using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Disconnect : MonoBehaviourPunCallbacks
{
    public GameObject Settings;
    public GameObject Pausa;

    //Voice Chat
    private AudioController voiceChat;
    private InputManager inputManager;
    
    void Start()
    {
      voiceChat = GameObject.Find("VoiceManager").GetComponent<AudioController>();  
      inputManager = GameObject.Find("Script").GetComponent<InputManager>();
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
      inputManager.OnreturnLobbyInput();
      voiceChat.onLeaveButtonClicked();
      
    }

}
            