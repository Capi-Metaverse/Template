using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoomPanel : MonoBehaviour, IMetaEvent
{
    [SerializeField] private CreateRoom createScript;
    GameObject _eventObject;
    GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }
    private GameManager gameManager;

    public void activate(bool host)
    {
        //We find the game manager
        gameManager = GameManager.FindInstance();
        //Open UI

        createScript.OpenUI();
    }



}
