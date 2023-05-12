using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class ChatGPTActive : MonoBehaviour, IMetaEvent
{
    public GameObject CanvasChatGPT;
    //PlayerSpawn playerSpawn;
    CharacterInputHandler CharacterInputHandler;
    GameObject eventObject;

    public void activate(bool host)
    {
        if (CharacterInputHandler == null) { CharacterInputHandler = GameManager.FindInstance().GetCurrentPlayer().gameObject.GetComponent<CharacterInputHandler>(); }
        if (host == true)
        {
            CanvasChatGPT.SetActive(true);
            CharacterInputHandler.DeactivateALL();
            GameObject.Find("adam").transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            CanvasChatGPT.SetActive(false);
            CharacterInputHandler.ActiveALL();
            GameObject.Find("adam").transform.GetChild(2).gameObject.SetActive(false);
        }
    }
    /*public void PressReturn()
    {
       CanvasChatGPT.SetActive(false); 
       playerSpawn.ActiveALL();
    }*/
}