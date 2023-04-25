using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class PauseMenuSettings : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject Settings;
    private GameObject Pause;
    [SerializeField] private TMP_Text roomName;

     PlayerList playerList;

    private void Start()
    {
        //We find the GameObjects
        Settings = GameObject.Find("Menus").transform.GetChild(0).gameObject;
        Pause = this.gameObject;
        gameManager = GameManager.FindInstance();
        roomName.text = gameManager.GetMapName();
    }

    //Method to disconnect the User
    public async void OnClickDisconnect()
    {
        await gameManager.Disconnect();
        SceneManager.LoadSceneAsync("1.Start");


    }

    //Method to open the settings menu
    public void OnClickSettings()
    {
        //Change visibility of menus
        Pause.SetActive(false);
        Settings.SetActive(true);


        //If the user is master  it will search the player list
         if (gameManager.GetUserRole() == UserRole.Admin)
            {
              playerList = GameObject.Find("TabPlayer").GetComponent<PlayerList>();
              playerList.ListPlayers();
            } 


    }

    //Method to return to the Lobby
    public async void OnClickReturnLobby()
    {
        await gameManager.Disconnect();
       
        SceneManager.LoadSceneAsync("Lobby");
       

    }

}
