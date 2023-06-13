using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelLobbyManager : MonoBehaviour
{
   
    public GameObject LobbyPanel;
    public GameObject RoomPanel;

    private LobbyManager lobbyManager;
    public void Start()
    {
        lobbyManager = this.GetComponent<LobbyManager>();
        Debug.Log(lobbyManager);
    }
    public void ChangeLobbyPanel()
    {
        if (RoomPanel != null & LobbyPanel != null)
        {
            RoomPanel.SetActive(false);
            LobbyPanel.SetActive(true);
        }
    }
    public void ChangeRoomPanel()
    {
        if (RoomPanel != null & LobbyPanel != null)
        {
            RoomPanel.SetActive(true);
            LobbyPanel.SetActive(false);
            lobbyManager.setLobbyButtons(false);
        }
    
}
}
