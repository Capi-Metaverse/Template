using Fusion;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.MultiplayerModels;

public class LobbyManager : MonoBehaviour
{
    //Name of the session/room
  


    //Game Manager of the game
    private GameManager gameManager;


    //List of rooms
    private List<RoomItem> sessionItemsList = new List<RoomItem>();


    public PanelLobbyManager PanelLobbyManager;

    public RoomPanelScript RoomPanelScript;

    public LobbyPanelScript LobbyPanelScript;
    //Avatar number

    private int avatarNumber = 0;
    

    /// <summary>
    /// When the Lobby awakes, it tries to find the game manager
    /// </summary>
    private void Awake()
    {
        gameManager = GameManager.FindInstance();

        gameManager.SetLobbyManager(this);
    }


    /// <summary>
    /// Function when the user creates a room/session.
    /// </summary>
    public void OnCreateSession(string sessionName)
    {
        Debug.Log("[Photon-LobbyManager] Creating session");
        //Properties of the room WIP
        SessionProps props = new SessionProps();
        props.StartMap = "Mapa1";
        props.RoomName = sessionName;
        props.AllowLateJoin = true;
        props.PlayerLimit = 10;


        gameManager.CreateSession(props);
    }

    /// <summary>
    /// Function when the user clicks on a session/room to join.
    /// </summary>
    /// <param name="sessionInfo"></param>
    public void OnJoinSession(SessionInfo sessionInfo)
    {
        Debug.Log("[Photon-LobbyManager] Joining session");
        gameManager.JoinSession(sessionInfo);

    }

 

    /// <summary>
    /// Function when the user clicks on a room to join
    /// </summary>
    public void OnJoinRoom(string sessionNamePanel)
    {
        //JoinButton.gameObject.SetActive(false);
        gameManager.StartGame(sessionNamePanel, avatarNumber);
    }

    /// <summary>
    /// Function to add the new sessions to the list of sessions
    /// </summary>
    /// <param name="sessionList"></param>
    public void SetSessionList(List<SessionInfo> sessionList)
    {
        //We clean the list
        Debug.Log("[Photon-LobbyManager] Setting session list");
        CleanSessions();
        //We instantiate the items in the interface.

        foreach (SessionInfo session in sessionList)
        {
            RoomItem newRoom = Instantiate(LobbyPanelScript.roomItemPrefab, LobbyPanelScript.contentObject);
            newRoom.SetSessionInfo(session);
            sessionItemsList.Add(newRoom);

        }

    }

    /// <summary>
    /// It sets the panel of players when you enter a session
    /// </summary>
    /// <param name="sessionName"></param>
    /// <param name="runner"></param>
    public void SetPlayerPanel(NetworkRunner runner)
    {
        
        PanelLobbyManager.ChangeRoomPanel();

        SpawnPlayerItem(runner, RoomPanelScript.playerItemPrefab);

    }

    /// <summary>
    /// It sets the LobbyPanel when the user leaves a session.
    /// </summary>
    public void SetLobbyPanel()
    {
        //We deactivate the panel of the room and Activate the panel of the Lobby interface.
        
            PanelLobbyManager.ChangeLobbyPanel();
        
    }

    /// <summary>
    /// Function when a user leaves the session
    /// </summary>
    public void OnLeaveSession()
    {
        SetLobbyPanel();
        gameManager.LeaveSession();
    }

   

    /// <summary>
    /// Function that cleans the session list
    /// </summary>
    public void CleanSessions()
    {

        foreach (RoomItem item in sessionItemsList)
        {
            if (item != null)
                Destroy(item.gameObject);
        }
        sessionItemsList.Clear();

    }

    /// <summary>
    /// Set true or false the create room button and the group of rooms
    /// </summary>
    /// <param name="active"></param>
    public void setLobbyButtons(bool active)
    {
        LobbyPanelScript.createButton.gameObject.SetActive(active);
        LobbyPanelScript.contentObject.gameObject.SetActive(active);
    }

    /// <summary>
    /// Sets the avatar that the player choose 
    /// </summary>
    /// <param name="number"></param>
    public void SetAvatarNumber(int number)
    {
        avatarNumber = number;
    }

    /// <summary>
    /// Spawn the PlayerItem in the Lobby Scene
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="player"></param>
    public void SpawnPlayerItem(NetworkRunner runner, PlayerItem player)
    {
        runner.Spawn(
        player,
        Vector3.zero,
        Quaternion.identity,
        inputAuthority: runner.LocalPlayer,
        BeforeSpawn,
        predictionKey: null
        );
    }

    /// <summary>
    /// Function that initializes the object on the first user
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="obj"></param>
    public static void BeforeSpawn(NetworkRunner runner, NetworkObject obj)
    {
        //We get the GameManager for username
        GameManager gameManager = GameManager.FindInstance();

        //We get the network object
        PlayerItem item = obj.GetComponent<PlayerItem>();

        //Set item
        item.setInfo(gameManager, runner, obj);
    }

}