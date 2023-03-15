using Fusion;

using System.Collections.Generic;
using TMPro;

using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private TMP_Text sessionName;
    private GameManager gameManager;

    private List<RoomItem> sessionItemsList = new List<RoomItem>();

    [SerializeField] private RoomItem roomItemPrefab;
    [SerializeField] private Transform contentObject;

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

}
