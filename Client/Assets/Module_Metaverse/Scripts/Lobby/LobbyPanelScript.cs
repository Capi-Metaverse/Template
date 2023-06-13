using Fusion;
using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanelScript : MonoBehaviour
{
 
    
    public TMP_Text sessionName;
    //Lobby panel
    public  RoomItem roomItemPrefab;
    public Transform contentObject;
    public Button createButton;

    private GameObject LobbyManager;

    private void Start()
    {
        LobbyManager = GameObject.Find("LobbyManager");
    }
    public void OnClickCreateSession()
    {
        if (LobbyManager.TryGetComponent(out LobbyManager lobbyManager))
        {
            lobbyManager.OnCreateSession(sessionName.text);
        }
    }
    public void OnClickJoinSession(SessionInfo sessionInfo)
    {
        if (LobbyManager.TryGetComponent(out LobbyManager lobbyManager))
        {
            lobbyManager.OnJoinSession(sessionInfo);
        }
    }
}
