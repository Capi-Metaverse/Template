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

    public SC_FPSController controller;
    private GameManagerTutorial gameManager;


    private void Start()
    {
        //We find the GameObjects
        gameManager = GameObject.Find("Manager").GetComponent<GameManagerTutorial>();
        Settings = GameObject.Find("Menus").transform.GetChild(0).gameObject;
        Pause = this.gameObject;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeactivatePauseMenu();
        }
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
        Debug.Log("OnClickSettings");
        Pause.SetActive(false);
        Settings.SetActive(true);

    }

    public void DeactivatePauseMenu()
    {
        controller.enabled = true;

        //Open pause menu and disable this
        gameManager.GameStatus = GameStatus.InGame;
        Pause.SetActive(false);
        controller.gameObject.SetActive(true);
        controller.micro.SetActive(true);
        controller.scope.SetActive(true);
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

    }
}
