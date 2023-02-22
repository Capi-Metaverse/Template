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
    PlayerList playerList;
    void Start()
    {
      voiceChat = GameObject.Find("VoiceManager").GetComponent<AudioController>();  
      
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
            
               
                Debug.Log("entro en leave");
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                PhotonNetwork.RaiseEvent(26,"", raiseEventOptions, SendOptions.SendUnreliable);

                // TODO: I need to show a popup message saying the master client left to the other clients
            
        }
    }


    public void OnClickSettings()
    {   
      Pausa.SetActive(false);
      Settings.SetActive(true);
      playerList = GameObject.Find("TabPlayer").GetComponent<PlayerList>();
      playerList.listadoJugadores();
      
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

}
            