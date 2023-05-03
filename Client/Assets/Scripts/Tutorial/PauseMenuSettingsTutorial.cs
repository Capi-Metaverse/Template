using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class PauseMenuSettingsTutorial : MonoBehaviour
{
    private GameObject Settings;
    private GameObject Pause;


    private void Start()
    {
        //We find the GameObjects
        Settings = GameObject.Find("Menus").transform.GetChild(0).gameObject;
        Pause = this.gameObject;
    }

    //Method to disconnect the User
    public void OnClickDisconnect()
    {
        SceneManager.LoadSceneAsync("1.Start");

    }

    //Method to open the settings menu
    public void OnClickSettings()
    {
        //Change visibility of menus
        Pause.SetActive(false);
        Settings.SetActive(true);

    }
}
