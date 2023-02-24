using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class SceneManagerScript : MonoBehaviourPunCallbacks
{
    //RoomNames
    public string[] ROOM_NAMES = { "Mapa0", "Mapa1", "Mapa2", "Mapa3" };

    public bool onMap = false; //Boolean that specifies if the user is on a Map or the Lobby

    public bool onNewMap = false; //Boolean that specifies if the user is going to a new map

    //Number of the actual map
    public int currentMapNumber = 0;

    public string currentMap; //Name of the map

    //Voice Chat
    private AudioController voiceChat;

    public Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();

    //Managers
    //LobbyManager
    public LobbyManager lobbyManager;

    //PlayerManager
    public PlayerSpawn playerManager;

    
    private InputManager inputManager;
    private MusicManager musicController;

    PlayerList playerList;
    public GameObject Settings;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        musicController = GameObject.Find("MusicManager").GetComponent<MusicManager>();
    }

    public Hashtable getRoomCustomSettings(int map)
    {
        //We create the new settings
        Hashtable customSettings = new Hashtable();

        //Map
        customSettings.Add("Map", map);
        customSettings.Add("Init", true);
        customSettings.Add("Name", currentMap);

        //Return the settings
        return customSettings;
    }

    public void createNewRoom(string mapName)
    {
        //Room CustomSettings

        Hashtable customSettings = getRoomCustomSettings(0);

        //Create options for the room
        RoomOptions opciones =
            new RoomOptions()
            {
                MaxPlayers = 4,
                BroadcastPropsChangeToAll = true,
                PublishUserId = true,
                CustomRoomProperties = customSettings,
                IsVisible = true,
                EmptyRoomTtl = 50000
            };

        //We join or create the new room
        PhotonNetwork.JoinOrCreateRoom(mapName, opciones, TypedLobby.Default);
    }

    //Photon state when a user connects to Photon
    public override void OnConnectedToMaster()
    {
        //It calls this method when we leave the room or change to another
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    //Photon state when a user connects to the Photon Lobby
    public override void OnJoinedLobby()
    {
        if (onMap != true)
        {
            //Activates on Lobby
            lobbyManager =
                GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
            voiceChat =
                GameObject.Find("VoiceManager").GetComponent<AudioController>();
            inputManager = 
                GameObject.Find("Script").GetComponent<InputManager>();   
                
        }

        //If we are changing the map, we have to join the new //RoomChanging
        if (onNewMap == true)
        {
            //Custom Room Options
            Hashtable customSettings = getRoomCustomSettings(currentMapNumber);

            //We stop communication to stop new scene events
            PhotonNetwork.IsMessageQueueRunning = false;

            //We change to a map
            onMap = true;
            PhotonNetwork
                .JoinOrCreateRoom(currentMap + "Map" + currentMapNumber,
                new RoomOptions()
                {
                    MaxPlayers = 4,
                    BroadcastPropsChangeToAll = true,
                    PublishUserId = true,
                    CustomRoomProperties = customSettings,
                    IsVisible = false
                },
                TypedLobby.Default);
            PhotonNetwork.LoadLevel(ROOM_NAMES[currentMapNumber]);

            //Voice Chat
            voiceChat.onMapChange();
            Debug.Log("Cambio");
            voiceChat
                .onJoinButtonClicked(currentMap + "Map" + currentMapNumber);
        }
    }

    //Photon state when a user joins to a room
    public override void OnJoinedRoom()
    {
        //If the user joins the CharacterSelector Screen
        if (onMap == false)
        {
            //We assign the mapName
            currentMap = PhotonNetwork.CurrentRoom.Name;

            //We set the RoomPanel
            lobbyManager.setRoomPanel();
        }
        else
        {
            //If the user joins another map, we get the playerManager and instantiate the player
            playerManager =
                GameObject.Find("PlayerSpawner").GetComponent<PlayerSpawn>();
            playerManager.instantiatePlayer();
        }
    }

    //Callback for when the connection to a room fails
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //Check return error to check for various failures
        Debug.Log (message);
        //Añadir errorText TO DO!!
        //errorText.text = "Ya te encuentras en esa sala";
    }

    //Photon CallBack called when leaving a room
    public override void OnLeftRoom()
    {
        //If onMap == false the user is on the Lobby
        if (onMap == false)
        {
            //If the user leaves a CharacterSelectionRoom
            if (onNewMap == false)
            {
                playerProperties["playerAvatar"] = 6;
                PhotonNetwork.SetPlayerCustomProperties (playerProperties);
  
                //We remove the list of players when leaving the room
                lobbyManager.setLobbyPanel();
            }
            //If the user joins to the LobbyRoom
            else
            {
                lobbyManager.setLoadingPanel();
            }
        }
        //If onMap == true the user is on a room
        else
        {
            //If the user wants to return the Lobby, he's not going to a NewMap
            if (onNewMap != true)
            {
                onMap = false;
                onNewMap = false;
                currentMapNumber = 0;
                inputManager.OnreturnLobbyInput();
                voiceChat.onLeaveButtonClicked();
                musicController.ActivateAudio();
                SceneManager.LoadScene("Lobby");
                
            }
        }
    }
     public override void OnPlayerEnteredRoom(Player newPlayer){
        Settings.SetActive(true);
        if(Settings.activeSelf == true){

        if (PhotonNetwork.IsMasterClient)
        {    
            playerList = GameObject.Find("Menus").transform.GetChild(1).GetChild(0).GetChild(3).GetComponent<PlayerList>();
            playerList.playerList();
        } 
        }
    }
    public override void OnPlayerLeftRoom(Player newPlayer){
        if(Settings.activeSelf == true){
        if (PhotonNetwork.IsMasterClient)
        { 
            playerList = GameObject.Find("Menus").transform.GetChild(1).GetChild(0).GetChild(3).GetComponent<PlayerList>();
            playerList.playerList();
        } 
        }
        Settings.SetActive(false);
    }

}
