using UnityEngine;
using TMPro;
using Fusion;


/// <summary>
/// This cript is attached to the RoomItem prefab that appears in the lists of rooms to join in the Lobby scene
/// and when the button is clicked the user enter into the room
/// </summary>
public class RoomItem : MonoBehaviour
{
    /*---------------------VARIABLES-----------------*/
    [SerializeField] private TMP_Text roomName;
    [SerializeField] private LobbyManager _lobbyManager;

    public SessionInfo sessionInfo { get; private set; }

    /*---------------------METHODS------------------*/
    private void Start()
    {
        _lobbyManager = FindObjectOfType<LobbyManager>();//We get the LobbyManager from scene
    }
    //public void SetRoomName(string _roomName)
    ////Set name of the room
    //{
    //    roomName.text = _roomName;
    //}

    /// <summary>
    /// Join a determinated room when pressed the room
    /// </summary>
    public void OnClickItem()
    //Enter into LobbyRoom
    {
        _lobbyManager.OnClickJoinSession(sessionInfo);
    }

    /// <summary>
    /// Sets the info of the RoomItem. Used by LobbyManager
    /// </summary>
    /// <param name="sessionInfo"></param>
    public void SetSessionInfo(SessionInfo sessionInfo)
    {
        this.sessionInfo = sessionInfo;
        roomName.text = sessionInfo.Name;
    }
}