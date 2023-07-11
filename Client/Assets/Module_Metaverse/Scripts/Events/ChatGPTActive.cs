using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Manager;

public class ChatGPTActive : MonoBehaviour, IMetaEvent
{
    public GameObject CanvasChatGPT;
    //PlayerSpawn playerSpawn;
    CharacterInputHandler CharacterInputHandler;

    GameObject _eventObject;
    GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }

    /// <summary>
    /// Event to activate Adam's UI and his camera
    /// </summary>
    public void activate(bool host)
    {
        if (CharacterInputHandler == null) { CharacterInputHandler = PhotonManager.FindInstance().CurrentPlayer.gameObject.GetComponent<CharacterInputHandler>(); }
        if (host == true)
        {
            CanvasChatGPT.SetActive(true);

            UIManager uiManager = UIManager.FindInstance();
            PauseManager pauseManager = PauseManager.FindInstance();
            pauseManager.Pause();
            uiManager.SetUIOff();

            //CharacterInputHandler.DeactivateALL();
            GameObject.Find("adam").transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            CanvasChatGPT.SetActive(false);
            //CharacterInputHandler.ActiveALL();
            GameObject.Find("adam").transform.GetChild(2).gameObject.SetActive(false);
        }
    }
   
}