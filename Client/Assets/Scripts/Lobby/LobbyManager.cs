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

public class LobbyManager : MonoBehaviourPunCallbacks
{
  
    //SceneManager
    public SceneManagerScript sceneManager;

    //New Room Configuration
    public string currentMap;

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
    public GameObject playButton;

    //Otras variables
    public Transform contentObject;
    public Transform playerItemParent;
    private void Start() 
    {
        //Find voice chat script and the scene manager
        sceneManager=GameObject.Find("SceneManager").GetComponent<SceneManagerScript>();
        voiceChat=GameObject.Find("VoiceManager").GetComponent<AudioController>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    

    //Buttons
    //Button method that creates a room.
    public void OnClickCreate()
    {
        //CurrentRoomName
            currentMap = roomInputField.text.ToUpper();
            sceneManager.createNewRoom(currentMap);
            //voiceChat.onJoinButtonClicked(currentMap);
    }

    //Method called by the RoomItem button to join a room
    public void OnClickJoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
        //voiceChat.onJoinButtonClicked(roomName);
        playButton.SetActive(true);
    }

    //Button method to leave the room.
    public void OnClickLeaveRoom()
    {
     PhotonNetwork.LeaveRoom();
     //voiceChat.onLeaveButtonClicked();
    }

    //Button method to Choose PJ
    public void OnClickPlayButton()
    {
        //We specificate if we are going to another map and leave the room
          
            Hashtable customSettings = PhotonNetwork.CurrentRoom.CustomProperties;
            int mapa = (int) customSettings["Map"];

            //We activate that we go to another map
            sceneManager.onNewMap = true;
            PhotonNetwork.LeaveRoom();
            //voiceChat.onLeaveButtonClicked();

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

    public void setRoomPanel()
    {
        
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        //We get the roomName
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

    public void setLobbyPanel()
    {
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

    public void setLoadingPanel(){
        roomPanel.SetActive(false);
        loadingPanel.SetActive(true);
    }

}