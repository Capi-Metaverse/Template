using System.Collections;
 using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Disconnect : MonoBehaviourPunCallbacks
{

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    public GameObject Settings;
    public GameObject Pausa;

    string mapName;
    string mapNumber;
    bool newMap = false;
    //Voice Chat
    private AudioController voiceChat;
    private InputManager inputManager;

     //Function to get the customSettings of a room
    public Hashtable getRoomCustomSettings(){

        //We create the new settings

         Hashtable customSettings = new Hashtable();

            //Map
            customSettings.Add("Map", 0);
            customSettings.Add("Init",true);
            customSettings.Add("Name",mapName);
            

        //Return the settings
        return customSettings;

    }

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
      PhotonNetwork.LeaveRoom();
      inputManager.OnreturnLobbyInput();
      voiceChat.onLeaveButtonClicked();
      
    }

    public override void OnLeftRoom()
    //When you leave the room set the user properties
    {
      Debug.Log("Leaving");
      if(newMap==false){
      playerProperties["playerAvatar"] = 6;
      PhotonNetwork.SetPlayerCustomProperties(playerProperties);
      }

    }

    public override void OnConnectedToMaster(){
       Debug.Log("Cuidao");
        Debug.Log(newMap);
      //You just go to lobby
      if(newMap == false){
      SceneManager.LoadScene("Lobby");
      }
      else {
        //You go to another room
        PhotonNetwork.JoinLobby();
        //

      }
    }

     public override void OnJoinedLobby()
    {

        //If we are changing the map, we have to join the new 
        if(newMap == true){

        //Custom Room Options
            
            Hashtable customSettings = getRoomCustomSettings();

            //We stop communication to stop new scene events
            PhotonNetwork.IsMessageQueueRunning = false;
            Debug.Log(mapNumber);
            Debug.Log(mapName + "Map" + mapNumber);
            PhotonNetwork.JoinOrCreateRoom(mapName + "Map" + mapNumber, new RoomOptions(){MaxPlayers = 4, BroadcastPropsChangeToAll = true, PublishUserId = true,CustomRoomProperties = customSettings, IsVisible = false},TypedLobby.Default);
            
            //Voice Chat
            
            voiceChat.onMapChange();
            voiceChat.onJoinButtonClicked(mapName + "Map" + mapNumber);
            
            int levelNumber = Convert.ToInt32(mapNumber) + 1;
            PhotonNetwork.LoadLevel("Mapa"+ levelNumber.ToString());
            //PhotonNetwork.IsMessageQueueRunning = true;
            //Voice Chat

            

        }
    }

    public void changeRoom(string Map){
       Debug.Log("EntroRoom");
      //Obtenemos el nombre del mapa
      mapName = (string) PhotonNetwork.CurrentRoom.CustomProperties["Name"];
      mapNumber = Map;
      newMap = true;

      PhotonNetwork.LeaveRoom();





    }
}
            