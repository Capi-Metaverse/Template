using Fusion;
using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyPanelScript : MonoBehaviour
{
 
    
    public TMP_Text sessionName;
    //Lobby panel
    public  RoomItem roomItemPrefab;
    public Transform contentObject;
    public Button createButton;
    public TMP_InputField roomNameField;
    private float delayInSeconds = 0.5f;
    private GameObject LobbyManager;

    private void Start()
    {
        LobbyManager = GameObject.Find("LobbyManager");
        EventSystem.current.SetSelectedGameObject(roomNameField.gameObject);
    }
    public void OnClickCreateSession()
    {
        createButton.interactable = false;
        StartCoroutine(EnableInteractableAfterDelay(delayInSeconds));
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

    public void OnCreateSessionEntered()
    {
        if (EventSystem.current.currentSelectedGameObject == roomNameField.gameObject && Input.GetKey(KeyCode.Return) && createButton.IsActive())
        {
            if (LobbyManager.TryGetComponent(out LobbyManager lobbyManager))
            {
                lobbyManager.OnCreateSession(sessionName.text);
            }
        }
    }
    IEnumerator EnableInteractableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        createButton.interactable = true;
    }
}
