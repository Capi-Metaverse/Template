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
    private PhotonManager photonManager;
    private GameObject Settings;
    private GameObject Pause;
    [SerializeField] private TMP_Text roomName;

    private void Start()
    {
        //We find the GameObjects
        Settings = GameObject.Find("Menus").transform.GetChild(0).gameObject;
        photonManager = PhotonManager.FindInstance();
        Pause = this.gameObject;
        gameManager = GameManager.FindInstance();
        roomName.text = PhotonManager.FindInstance().RoomName;
    }

    /// <summary>
    /// Method to disconnect the User and return to Login
    /// </summary>
    public async void OnClickDisconnect()
    {
        await photonManager.Disconnect();
        SceneManager.LoadSceneAsync("1.Start");


    }

    //Method to open the settings menu
    public void OnClickSettings()
    {
        //Change visibility of menus
        Pause.SetActive(false);
        Settings.SetActive(true);

    }

    //Method to return to the Lobby
    public async void OnClickReturnLobby()
    {
        await photonManager.Disconnect();
       
        SceneManager.LoadSceneAsync("Lobby");
       

    }

}
