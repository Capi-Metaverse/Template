using Fusion;

using System.Collections.Generic;
using TMPro;

using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private TMP_Text sessionName;
    private GameManager gameManager;

    //List of rooms
    private List<RoomItem> sessionItemsList = new List<RoomItem>();

    //List of players
    private List<PlayerItem> playerItemsList = new List<PlayerItem>();

    //Lobby panel
    [SerializeField] private RoomItem roomItemPrefab;
    [SerializeField] private Transform contentObject;
    [SerializeField] private GameObject lobbyPanel;
   

    //Players panel
    [SerializeField] private PlayerItem playerItemPrefab;
    [SerializeField] private GameObject roomPanel;

    [SerializeField] private Transform playerItemParent;

    private void Awake()
    {
        gameManager = GameManager.FindInstance();
        gameManager.setLobbyManager(this);
        
    }
    public void OnClickJoinSession(SessionInfo sessionInfo)
    {

        gameManager.JoinSession(sessionInfo);
        
    }

    public void OnClickCreateSession()
    {
        Debug.Log("Creating session");
        SessionProps props = new SessionProps();
        props.StartMap = "Mapa0";
        props.RoomName = sessionName.text;
        props.AllowLateJoin = true;
        
        gameManager.CreateSession(props);
    }

    public void addSession(SessionInfo sessionInfo)
    {
        //We instantiate the item in the interface.
        RoomItem newRoom = Instantiate(roomItemPrefab, contentObject);
        if (newRoom != null)
        {
            newRoom.SetSessionInfo(sessionInfo);
            sessionItemsList.Add(newRoom);
        }

    }

    public void deleteSession(SessionInfo sessionInfo)
    {
        int index = sessionItemsList.FindIndex(x => x.sessionInfo.Name == sessionInfo.Name);
        if (index != -1)
        {
            Destroy(sessionItemsList[index].gameObject);
            sessionItemsList.RemoveAt(index);
        }

    }

    public void setPlayerPanel()
    {

        //We deactivate the panel of the room and Activate the panel of the Lobby interface.
        if (roomPanel != null & lobbyPanel != null)
        {
            roomPanel.SetActive(true);
            lobbyPanel.SetActive(false);
        }

    }

    public void setLobbyPanel()
    {
        foreach (PlayerItem item in playerItemsList)
        {
            if (item != null)
                Destroy(item.gameObject);
        }
        playerItemsList.Clear();

        //We deactivate the panel of the room and Activate the panel of the Lobby interface.
        if (roomPanel != null & lobbyPanel != null)
        {
            roomPanel.SetActive(false);
            lobbyPanel.SetActive(true);
        }
    }

    public void addPlayer(PlayerRef player)
    {

        gameManager.spawnPlayerItem(playerItemPrefab);
    

        //Sets (only for the session player and not for the others in the room) the visible arrows to be able to select the avatar in the selector
        
        //playerItemsList.Add(playerItem);
    }

}
