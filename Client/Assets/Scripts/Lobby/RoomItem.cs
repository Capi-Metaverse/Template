
using UnityEngine;
using TMPro;
using Fusion;

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
    public void SetRoomName(string _roomName)
    //Set name of the room
    {
        roomName.text = _roomName;
    }
    public void OnClickItem()
    //Enter into LobbyRoom
    {
        _lobbyManager.OnClickJoinSession(sessionInfo);
    }

    public void SetSessionInfo(SessionInfo sessionInfo)
    {
        this.sessionInfo = sessionInfo;
        roomName.text = sessionInfo.Name;
    }
}