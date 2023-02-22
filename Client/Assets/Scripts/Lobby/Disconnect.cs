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
    private MusicManager musicController;
    
    void Start()
    {
      voiceChat = GameObject.Find("VoiceManager").GetComponent<AudioController>();  
      inputManager = GameObject.Find("Script").GetComponent<InputManager>();
      musicController = GameObject.Find("MusicManager").GetComponent<MusicManager>();
    }

    public void OnClickDisconnect()
    {   
        PhotonNetwork.Disconnect();
       
        voiceChat.OnApplicationQuit();
        Application.Quit();
    }
    
    public void Leave()
    {
      Debug.Log("Funciona");
      Debug.Log(PhotonNetwork.IsMasterClient);
    if (PhotonNetwork.IsMasterClient == true)
        {
          Debug.Log("entro en leave");
            Player[] otherPlayers = PhotonNetwork.PlayerListOthers;
            for (int i = 0; i < otherPlayers.Length; ++i)
            {
                Debug.Log(otherPlayers[i]);
                Debug.Log("entro en leave");
                PhotonNetwork.CloseConnection(otherPlayers[i]);
                // TODO: I need to show a popup message saying the master client left to the other clients
            }
        }
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
      musicController.ActivateAudio();
      
    }

}
            