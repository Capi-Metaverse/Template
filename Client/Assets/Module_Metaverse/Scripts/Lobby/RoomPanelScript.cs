using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Manager;
public class RoomPanelScript : MonoBehaviour
{
    public Button createButton;
    public TMP_Text sessionNamePanel;
    public PlayerItem playerItemPrefab;
    [SerializeField] private Button JoinButton;

    private GameObject LobbyManager;

    private void Start()
    {
        LobbyManager = GameObject.Find("LobbyManager");
    }

    public void OnClickJoinRoom()
    {
        if (LobbyManager.TryGetComponent(out LobbyManager lobbyManager))
        {
            JoinButton.interactable = false;
            lobbyManager.OnJoinRoom(sessionNamePanel.text);
        }
    }
    public void OnClickLeaveSession()
    {
        createButton.interactable = true;
        if (LobbyManager.TryGetComponent(out LobbyManager lobbyManager))
        {
            lobbyManager.OnLeaveSession();
        }
    }
}
