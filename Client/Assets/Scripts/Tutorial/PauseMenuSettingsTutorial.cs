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
    private GameObject Pause;

    public SC_FPSController controller;
    private GameManagerTutorial gameManager;

    [SerializeField] private Dialogue dialogueScript;

    private bool isTutorial = false;


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
        if (isTutorial == false)
        {
            //Change visibility of menus
      
            Pause.SetActive(false);
            Settings.SetActive(true);
        }

        else
        {
            Pause.SetActive(false);
            Settings.SetActive(true);
            Settings.GetComponentInChildren<MoveTabsTutorial>().StartTutorial();
            isTutorial = false;
        }

    }

    public void StartTutorial()
    {
        //Diálogo
        string[] lines = new string[2] { "This is the pause menu. You can disconnect from the application here.", "You can enter the settings menu from here too! Click on the gear icon in the top of the panel." };
        dialogueScript.lines = lines;
        dialogueScript.textComponent.text = string.Empty;
        dialogueScript.gameObject.SetActive(true);

        dialogueScript.StartDialogue();



        isTutorial = true;

    }

    public void DeactivatePauseMenu()
    {
        controller.enabled = true;

        //Open pause menu and disable this
        gameManager.GameStatus = GameStatus.InGame;
        Pause.SetActive(false);
        controller.gameObject.SetActive(true);
        controller.playerUI.ShowUI();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
}
