using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Disconnect : MonoBehaviourPunCallbacks
{

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    public GameObject Settings;
    public GameObject Pausa;
    //Voice Chat
    private AudioController voiceChat;

    void Start(){
      voiceChat = GameObject.Find("VoiceManager").GetComponent<AudioController>();
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
      PhotonNetwork.LeaveRoom();
      voiceChat.onLeaveButtonClicked();
    }

    public override void OnLeftRoom()
    //When you leave the room set the user properties
    {
      Debug.Log("Leaving");
      playerProperties["playerAvatar"] = 6;
      PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }
    public override void OnConnectedToMaster(){
      SceneManager.LoadScene("Lobby");
    }
}
            