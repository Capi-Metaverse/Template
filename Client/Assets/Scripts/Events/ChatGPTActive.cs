using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ChatGPTActive : MonoBehaviour, IMetaEvent
{
    public GameObject CanvasChatGPT;
    PlayerSpawn playerSpawn;
   
    private void Start() 
    {
        playerSpawn = GameObject.FindObjectOfType<PlayerSpawn>();
    }
    public void activate(bool host)
    {
        if (host == true)
        {
            CanvasChatGPT.SetActive(true);    
            playerSpawn.DesactiveALL();       
        }  
        else 
        {
            CanvasChatGPT.SetActive(false); 
            playerSpawn.ActiveALL();
        } 
    }
    /*public void PressReturn()
    {
       CanvasChatGPT.SetActive(false); 
       playerSpawn.ActiveALL();
    }*/
}
