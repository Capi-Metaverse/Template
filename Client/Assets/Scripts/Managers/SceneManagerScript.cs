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

public class SceneManagerScript : MonoBehaviourPunCallbacks
{
    //RoomNames
    public string[] ROOM_NAMES = {"Mapa1", "Mapa2","Lobby"};
    

    public bool onMap = false; //Boolean that specifies if the user is on a Map
    public bool onNewMap = false; //Boolean that specifies if the user is on a Map
    
    //Number of the actual map
    public int currentMapNumber;
    public string currentMap;

     //Voice Chat
    private AudioController voiceChat;

    public Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();

   

    //Managers
    //LobbyManager
    public LobbyManager lobbyManager;
    //PlayerManager
    public PlayerSpawn playerManager;

    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        //voiceChat=GameObject.Find("VoiceManager").GetComponent<AudioController>();
    }

    public Hashtable getRoomCustomSettings(int map){
        //We create the new settings

         Hashtable customSettings = new Hashtable();

            //Map
            customSettings.Add("Map", map);
            customSettings.Add("Init",true);
            customSettings.Add("Name",currentMap);

            
        //Return the settings
        return customSettings;
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

        if(onMap != true){
            lobbyManager=GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
            voiceChat=GameObject.Find("VoiceManager").GetComponent<AudioController>();
        }  
        Debug.Log("Entro aqui33");

       
        //If we are changing the map, we have to join the new //PART FROM LOBBY
        if(onNewMap == true ){

            //Custom Room Options
            Hashtable customSettings = getRoomCustomSettings(currentMapNumber);

            //We stop communication to stop new scene events
            PhotonNetwork.IsMessageQueueRunning = false;
            //We change to a map
            onMap = true;
            PhotonNetwork.JoinOrCreateRoom(currentMap + "Map" + currentMapNumber, new RoomOptions(){MaxPlayers = 4, BroadcastPropsChangeToAll = true, PublishUserId = true,CustomRoomProperties = customSettings, IsVisible = false},TypedLobby.Default);
            PhotonNetwork.LoadLevel(ROOM_NAMES[currentMapNumber] );

            //Voice Chat
            voiceChat.onMapChange();
            Debug.Log("Cambio");
            voiceChat.onJoinButtonClicked(currentMap+"Map"+currentMapNumber);
        }
    }

    //Photon state when a user joins to a room
    public override void OnJoinedRoom()
    {
        
        //Lobby
        if(onMap == false){

        currentMap = PhotonNetwork.CurrentRoom.Name;

        lobbyManager.setRoomPanel();
    }

    else{
        playerManager = GameObject.Find("PlayerSpawner").GetComponent<PlayerSpawn>();
        playerManager.instantiatePlayer();
    }
    }

    //Callback for when the connection to a room fails
    public override void OnJoinRoomFailed(short returnCode, string message){

        //Check return error to check for various failures
        Debug.Log(message);
        //Añadir errorText TO DO!!
        //errorText.text = "Ya te encuentras en esa sala";
    }


    //Photon CallBack called when leaving a room
    public override void OnLeftRoom()
    {
        
        //Changes if we aren't leaving the map
        if(onMap == false){
        if (onNewMap == false)
        {
            playerProperties["playerAvatar"] = 6;
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
            //We remove the list of players when leaving the room
            lobbyManager.setLobbyPanel();
        }

        else{
            lobbyManager.setLoadingPanel();
        }
        }
        else{
            Debug.Log("Entro aqui");
            if(onNewMap != true){
                   Debug.Log("Aqui no");
                onMap = false;
                onNewMap = false;
                SceneManager.LoadScene("Lobby");
                 

            }


        }
    }





}
