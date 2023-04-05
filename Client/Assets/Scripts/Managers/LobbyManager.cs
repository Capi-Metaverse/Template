using Fusion;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    //Name of the session/room
    [SerializeField] private TMP_Text sessionName;
    [SerializeField] private TMP_Text sessionNamePanel;

    //Game Manager of the game
    private GameManager gameManager;


    //List of rooms
    private List<RoomItem> sessionItemsList = new List<RoomItem>();

    //Lobby panel
    [SerializeField] private RoomItem roomItemPrefab;
    [SerializeField] private Transform contentObject;
    [SerializeField] private GameObject lobbyPanel;
   

    //Players panel
    [SerializeField] private PlayerItem playerItemPrefab;
    [SerializeField] private GameObject roomPanel;

    [SerializeField] private Transform playerItemParent;

    //LobbyButtons
    [SerializeField] private Button createButton;

    //Avatar number

    private int avatarNumber = 0;


    //When the Lobby awakes, it tries to find the game manager
    private void Awake()
    {
        gameManager = GameManager.FindInstance();
       
        gameManager.SetLobbyManager(this);
        
     

        
    }

    //Function when the user clicks on a session/room to join.
    public void OnClickJoinSession(SessionInfo sessionInfo)
    {
        Debug.Log("Joining session");
        gameManager.JoinSession(sessionInfo);
        
    }

    //Function when the user creates a room/session.
    public void OnClickCreateSession()
    {
        Debug.Log("Creating session");
        //Properties of the room WIP
        SessionProps props = new SessionProps();
        props.StartMap = "HUBValencia";
        props.RoomName = sessionName.text;
        props.AllowLateJoin = true;
        props.PlayerLimit = 10;
        
        gameManager.CreateSession(props);
    }

    //Function to add the new sessions to the list of sessions
    public void SetSessionList(List<SessionInfo> sessionList)
    {
        //We clean the list
        Debug.Log("Setting session list");
        CleanSessions();
        //We instantiate the items in the interface.

        foreach(SessionInfo session in sessionList)
        {
            RoomItem newRoom = Instantiate(roomItemPrefab, contentObject);
            newRoom.SetSessionInfo(session);
            sessionItemsList.Add(newRoom);

        }
     
    }

    


    //It sets the panel of players when you enter a session
    public void SetPlayerPanel(string sessionName, NetworkRunner runner)
    {
        this.sessionNamePanel.text = sessionName;
        this.sessionName.text = sessionName;
        //We deactivate the panel of the room and Activate the panel of the Lobby interface.
        if (roomPanel != null & lobbyPanel != null)
        {
            roomPanel.SetActive(true);
            lobbyPanel.SetActive(false);
            setLobbyButtons(false);
        }

        SpawnPlayerItem(runner,playerItemPrefab);

    }

    //It sets the LobbyPanel when the user leaves a session.
    public void SetLobbyPanel()
    {
        //We deactivate the panel of the room and Activate the panel of the Lobby interface.
        if (roomPanel != null & lobbyPanel != null)
        {
            roomPanel.SetActive(false);
            lobbyPanel.SetActive(true);
        }
    }

    //Function when a user leaves the session
    public void OnClickLeaveSession()
    {
        SetLobbyPanel();
        gameManager.LeaveSession();

    }

    public void OnClickJoinRoom()
    {
        Debug.Log(sessionName.text);
        gameManager.StartGame(sessionNamePanel.text,avatarNumber);
    }

    //Function that cleans the session list
    public void CleanSessions()
    {

        foreach (RoomItem item in sessionItemsList)
        {
            if (item != null)
                Destroy(item.gameObject);
        }
        sessionItemsList.Clear();
       
    }

    public void setLobbyButtons(bool active)
    {
        createButton.gameObject.SetActive(active);
        contentObject.gameObject.SetActive(active);
    }

 public void SetAvatarNumber(int number)
    {
        avatarNumber = number;
    }




    //PlayerItemSpawn
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

    //Function that initializes the object on the first user
    public static void BeforeSpawn(NetworkRunner runner, NetworkObject obj)
    {
        //We get the GameManager for username
        GameManager gameManager = GameManager.FindInstance();

        //We get the network object
        PlayerItem item = obj.GetComponent<PlayerItem>();

        //Set item
        item.setInfo(gameManager,runner,obj);
        


       

    }

}
