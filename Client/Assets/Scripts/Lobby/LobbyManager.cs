using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyManager : MonoBehaviourPunCallbacks, IOnEventCallback
{

    //Constantes
    public const int NUMBER_MAPS = 2;

    public string[] ROOM_NAMES = {"Mapa1", "Mapa2"};

    //Variables
    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();

    //Paneles UI
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    public GameObject loadingPanel;
    public TMP_Text mapName;
    //Inputs
    //Input para crear una sala
    public TMP_InputField roomInputField;
    
    //Text Labels
    public TMP_Text errorText;
    public TMP_Text roomName;

    //Voice Chat
    private AudioController voiceChat;

    //Listas
    List<RoomItem> roomItemsList = new List<RoomItem>();
    public List<PlayerItem> playerItemsList = new List<PlayerItem>();

    //Prefabs

    public RoomItem roomItemPrefab;
    public PlayerItem playerItemPrefab;

    //Buttons

    public GameObject leftArrowButton;
    public GameObject rightArrowButton;
    public GameObject playButton;

    //Otras variables
    public Transform contentObject;
    public Transform playerItemParent;

  



    private void Start() 
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);

        //find voice chat script
        voiceChat=GameObject.Find("VoiceManager").GetComponent<AudioController>();
    }
    

    
    private void Update() 
    {
        {
            //Activate panels
            if (PhotonNetwork.CurrentRoom != null )
            {
                
                if (PhotonNetwork.IsMasterClient || (bool) PhotonNetwork.CurrentRoom.CustomProperties["Init"])
                {
                    playButton.SetActive(true);
                }
                else
                {
                    playButton.SetActive(false);
                }
                if (PhotonNetwork.IsMasterClient)
                {
                    mapName.enabled = true;
                    leftArrowButton.SetActive(true);
                    rightArrowButton.SetActive(true);
                }
                else 
                {
                    mapName.enabled = false;
                    leftArrowButton.SetActive(false);
                    rightArrowButton.SetActive(false);
                }
            }
        }
    }

    //Buttons

    //Button method that creates a room.
    public void OnClickCreate()
    {
        if(roomInputField.text.Length >=1)
        {

            //Custom Room Options

            Hashtable customSettings = new Hashtable();

            //Map
            customSettings.Add("Map", 0);
            customSettings.Add("Init",false);

            //Create options for the room

            RoomOptions opciones = new RoomOptions(){MaxPlayers = 4, BroadcastPropsChangeToAll = true, PublishUserId = true, CustomRoomProperties = customSettings};

            PhotonNetwork.CreateRoom(roomInputField.text, opciones);
            voiceChat.onJoinButtonClicked(roomInputField.text);
        }
    }

    //Method called by the RoomItem button to join a room
    public void OnClickJoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
        voiceChat.onJoinButtonClicked(roomName);
    }

    //Button method to leave the room.
    public void OnClickLeaveRoom()
    {
     PhotonNetwork.LeaveRoom();
     voiceChat.onLeaveButtonClicked();
    }

    //Button method to Choose PJ
    public void OnClickPlayButton()
    {
        //If it is the main client, it sends an event to the others so that they can be synchronized
        if (PhotonNetwork.IsMasterClient)
        {
            //Send event
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(3, "", raiseEventOptions, SendOptions.SendReliable);

            //Change room to start

            Hashtable customSettings = PhotonNetwork.CurrentRoom.CustomProperties;
            customSettings["Init"] = true;
            PhotonNetwork.CurrentRoom.SetCustomProperties(customSettings);
        }
        else
        {
           //We send event if we join after
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others }; // You would have to set the Receivers to All in order to receive this event on the local client as well
            PhotonNetwork.RaiseEvent(1, "", raiseEventOptions, SendOptions.SendReliable);
             
            //PhotonNetwork.IsMessageQueueRunning = false;
            int mapa = (int) PhotonNetwork.CurrentRoom.CustomProperties["Map"];
            PhotonNetwork.LoadLevel(ROOM_NAMES[mapa] );
        }
    } 


    //Button method to switch the map to the left.
    public void OnClickChangeMapLeft()
    {
        //We get the properties of the room
        Hashtable customSettings = PhotonNetwork.CurrentRoom.CustomProperties;
        int mapa = (int) customSettings["Map"];
          
        //If we are already in the last map we do not change it
        if( mapa == 0) return;

        customSettings["Map"] = mapa - 1;
        mapName.text = ROOM_NAMES[mapa - 1];
        PhotonNetwork.CurrentRoom.SetCustomProperties(customSettings);
    }
    //Button method to switch the map to the right.
    public void OnClickChangeMapRight()
    {
        //We get the properties of the room

        Hashtable customSettings = PhotonNetwork.CurrentRoom.CustomProperties;
        int mapa = (int) customSettings["Map"];
           
        //If we are already in the last map we do not change it
        if( mapa == (NUMBER_MAPS - 1) ) return;

        customSettings["Map"] = mapa + 1;
        mapName.text = ROOM_NAMES[mapa + 1];
        PhotonNetwork.CurrentRoom.SetCustomProperties(customSettings);
     }
    //Funciones 

    //Function to add players to the list of players that are already in the room

    public void AddPlayer(Player newPlayer)
    {
        //We instantiate the new player and add it to the user list
        PlayerItem playerItem = Instantiate(playerItemPrefab, playerItemParent); 
        playerItem.SetPlayerInfo(newPlayer);
       
        //Sets (only for the session player and not for the others in the room) the visible arrows to be able to select the avatar in the selector
        if (newPlayer == PhotonNetwork.LocalPlayer)
        {
            playerItem.ApplyLocalChanges();//Method of PlayerItem.cs, only does a SetActive(true) on the selection arrows
        }
        playerItemsList.Add(playerItem);
    }

    //Function that deletes the players from the list
   
    public void DeletePlayer(Player oldPlayer)
    {
        //Go through the list and check the ID's of the users to delete it
        foreach(PlayerItem item in playerItemsList)
        {
           if (item.GetPlayerInfo().UserId == oldPlayer.UserId)
           {
                Destroy(item.gameObject);
                playerItemsList.Remove(item);
           }
        }
    }

    //Method when a user enters a room for the first time, it updates the list of players already in the room
    void UpdatePlayerList()
    {
        foreach (KeyValuePair<int,Player> player in PhotonNetwork.CurrentRoom.Players)
        {
           PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent); 
           newPlayerItem.SetPlayerInfo(player.Value);

            if (player.Value == PhotonNetwork.LocalPlayer)
            {
                newPlayerItem.ApplyLocalChanges();
            }

            playerItemsList.Add(newPlayerItem);
        }
    }

    //Callbacks Photon


    /*
    **********

    IMPORTANTE
    COMO FUNCIONA CONEXIÓN PHOTON
    MASTER SERVER (ConnectUsingSettings) -> GAME SERVER (JoinLobby) -> ROOM (JoinOrCreateRoom)

    **********
    */

    //Photon CallBack that is called once it connects to the main server
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    //Photon CallBack that is called every x time with new information from the lists.
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //We go through the new list with information
        foreach (RoomInfo info in roomList)
        {
            //If a room has been destroyed we remove it from the list and from the interface
            if (info.RemovedFromList)
            {
                int index = roomItemsList.FindIndex( x => x.RoomInfo.Name == info.Name);
                if (index != -1)
                {
                    Destroy(roomItemsList[index].gameObject);
                    roomItemsList.RemoveAt(index);
                }
            }
            else
            {
                //Test if this if is needed.
                if (roomItemsList.FindIndex( x => x.RoomInfo.Name == info.Name) == -1)
                {
                    //We instantiate the item in the interface.
                    RoomItem newRoom = Instantiate(roomItemPrefab,contentObject);
                    if (newRoom != null)
                    {
                        newRoom.SetRoomInfo(info);
                        roomItemsList.Add(newRoom);
                    }
                }
            }
        }
    }

    //Photon CallBack called when user joins a room.
    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text=PhotonNetwork.CurrentRoom.Name;

        //Method to update the player list

        UpdatePlayerList();

        //We destroy the list of rooms as it is not updated and will be updated when we join the Lobby again.

         foreach(RoomItem item in roomItemsList)
        {
            Destroy(item.gameObject);
        }
        roomItemsList.Clear();    
    } 

    //Callback for when the connection to a room fails
    public override void OnJoinRoomFailed(short returnCode, string message){

        //Check return error to check for various failures
        Debug.Log(message);
        errorText.text = "Ya te encuentras en esa sala";
    }

    //Photon CallBack called when leaving a room
    public override void OnLeftRoom()
    {
        playerProperties["playerAvatar"] = 6;
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        //We remove the list of players when leaving the room

        foreach(PlayerItem item in playerItemsList)
        {
            if (item!=null)
            Destroy(item.gameObject);
        }
        playerItemsList.Clear();

        //We deactivate the panel of the room and Activate the panel of the Lobby interface.
        if (roomPanel!=null & lobbyPanel!=null){
            roomPanel.SetActive(false);
            lobbyPanel.SetActive(true);
        }
    }

    //Photon CallBack when a player enters the room.
    public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            AddPlayer(newPlayer);
        }

    //Photon CallBack when a player leaves the room.
    public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            DeletePlayer(otherPlayer);
        }

    public void OnEvent(EventData photonEvent)
    {
         
        if(photonEvent.Code == 3)
        {
            roomPanel.SetActive(false);
            loadingPanel.SetActive(true);
            
           
            PhotonNetwork.IsMessageQueueRunning = false;
            int mapa = (int) PhotonNetwork.CurrentRoom.CustomProperties["Map"];
            PhotonNetwork.LoadLevel(ROOM_NAMES[mapa] );
        }
    }
    
}