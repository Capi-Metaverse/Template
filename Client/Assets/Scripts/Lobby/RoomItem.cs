using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;

public class RoomItem : MonoBehaviour
{
    /*---------------------VARIABLES-----------------*/
    public TMP_Text roomName;

    public SessionInfo sessionInfo { get; private set; }
    LobbyManager manager;

    /*---------------------METHODS------------------*/
    private void Start()
    {
        manager = FindObjectOfType<LobbyManager>();//We get the LobbyManager from scene
    }
    public void SetRoomName(string _roomName)
    //Set name of the room
    {
        roomName.text = _roomName;
    }
    public void OnClickItem()
    //Enter into LobbyRoom
    {
        manager.OnClickJoinSession(sessionInfo);
    }

    public void SetSessionInfo(SessionInfo sessionInfo)
    {
        this.sessionInfo = sessionInfo;
        roomName.text = sessionInfo.Name;
    }
}