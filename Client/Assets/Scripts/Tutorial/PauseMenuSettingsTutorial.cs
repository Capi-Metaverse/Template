using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using static System.Windows.Forms.LinkLabel;


public class PauseMenuSettingsTutorial : MonoBehaviour
{
    private GameObject Settings;
 

    public SC_FPSController controller;
    private GameManagerTutorial gameManager;

    [SerializeField] private Dialogue dialogueScript;


    private void Start()
    {
        //We find the GameObjects
        gameManager = GameObject.Find("ManagerTutorial").GetComponent<GameManagerTutorial>();
        Settings = GameObject.Find("Menus").transform.GetChild(0).gameObject;
 
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (gameManager.TutorialStatus != TutorialStatus.Settings) && (gameManager.TutorialStatus != TutorialStatus.PreSettings))
        {
            Hide();
        }
    }

    //Method to disconnect the User
    public void OnClickDisconnect()
    {
        SceneManager.LoadSceneAsync("1.Start");

    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    //Method to open the settings menu
    public void OnClickSettings()
    {
        Settings.SetActive(true);
        if (gameManager.TutorialStatus == TutorialStatus.Settings) gameManager.CompleteObjective(0);
        Hide();

    }


    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
    /*
    public void DeactivatePauseMenu()
    {
        controller.enabled = true;

        //Open pause menu and disable this
        gameManager.GameStatus = GameStatus.InGame;
        this.gameObject.SetActive(false);
        controller.gameObject.SetActive(true);
        controller.playerUI.ShowUI();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
    */
}
