using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Windows.Forms.LinkLabel;



public enum DialogueStatus
{
    InDialogue,
    InGame
}

public enum GameStatus
{
    InPause,
    InGame,
}

public enum SettingsStatus
{
    None,
    Settings,
    Keys,
    Friends,
    Players,
    Finished
}
public enum TutorialStatus
{
    Movement,
    Jumping,
    Interaction,
    Voice,
    PreSettings,
    Settings,
    Presentation,
    Animations,
    Finished
}

public class GameManagerTutorial : MonoBehaviour
{
    //TutorialStatus
    private TutorialStatus tutorialStatus;
    public TutorialStatus TutorialStatus { get => tutorialStatus; set => tutorialStatus = value; }

    //DialogueStatus
    private DialogueStatus dialogueStatus;
    public DialogueStatus DialogueStatus { get => dialogueStatus; set => dialogueStatus = value; }

    //GameStatus
    private GameStatus gameStatus;
    public GameStatus GameStatus { get => gameStatus; set => gameStatus = value; }

    //SettingStatus
    private SettingsStatus settingsStatus = SettingsStatus.None;
    public SettingsStatus SettingsStatus { get => settingsStatus; set => settingsStatus = value; }


    [SerializeField] private Dialogue dialogueScript;
    [SerializeField] private SC_FPSController fpsController;
    [SerializeField] private MoveTabsTutorial moveTabs;
    [SerializeField] private Animator animator;
    [SerializeField] private TMP_Text reShowText;
    [SerializeField] private PlayerUI playerUI;


    //Tutorial UI
    [SerializeField] private TMP_Text[] objectives;

    [SerializeField] private TMP_Text tutorialNumber;

    private int[] objectiveCounter = new int[3];

    [SerializeField] private PauseMenuSettingsTutorial pauseMenuSettings;

    public void Start()
    {
        tutorialStatus = TutorialStatus.Movement;
        dialogueStatus = DialogueStatus.InDialogue;
        gameStatus = GameStatus.InGame;


        //Init first Dialogue
        StartDialogue();
        
    }


    private void OnDialogueStatus()
    {
        //SetDialogueStatus
        DialogueStatus = DialogueStatus.InDialogue;

        //Make cursor visible
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //Deactive animation
        animator.SetFloat("Speed", 0);

        //Deactive UI && Movement
        playerUI.HideUI();
        fpsController.enabled = false;
        reShowText.enabled = false;

    }

    public void OnInGameStatus()
    {
        if (TutorialStatus != TutorialStatus.Settings)
        {
            //SetDialogueStatus
            DialogueStatus = DialogueStatus.InGame;


            //Make cursor invisible
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            //Active animation
            animator.SetFloat("Speed", 1);

            //Active UI && Movement
            playerUI.ShowUI();
            fpsController.enabled = true;
            reShowText.enabled = true;
        }
    }

    public void StartDialogue()
    {
        OnDialogueStatus();
        string[] lines = { "Empty" };
        EmptyObjectives();

        switch (tutorialStatus)
        {
            case TutorialStatus.Movement:
                lines = new string[3] { " I'm your virtual assistant, Adam.", "I'm here to help you with the basics of this application.", "You can move to any direction with the WASD Keys, Try it!" };

                //Objectives
                objectives[0].text = "Press W to move forward.";
                objectives[1].text= "Press S to move backwards";
                objectives[2].text = "Press A to move to the left.";
                objectives[3].text = "Press D to move to the right.";
                break;

            case TutorialStatus.Jumping:
               
                lines = new string[2] { "Congratulations! In the metaverse you can jump too.", "Go upstairs and press Space to Jump" };

                //Objectives
                objectives[0].text = "Press space to jump.";
                tutorialNumber.text = "2";
                break;

            case TutorialStatus.Interaction:
                
                lines = new string[2] { "Well Done! Now, let's do something more interactive.", "Go near a lamp and press e to interact with it" };

                //Objectives
                objectives[0].text = "Press e with an interactable object. 0/2";
                tutorialNumber.text = "3";
                break;

            case TutorialStatus.Voice:
                
                lines = new string[2] { "Now, let's see how to talk with someone.", "Press M to mute and unmute your voice" };

                //Objectives
                objectives[0].text = "Press m to mute/unmute the chat 0/2";
                tutorialNumber.text = "4";
                break;

            case TutorialStatus.PreSettings:
                lines = new string[2] { "After turning the voice on/off, it's time to see how to modify the settings", "Press ESC to show the Pause and Settings Menus" };
                objectives[0].text = "Press ESC to open the pause menu";
                tutorialNumber.text = "5";
                break;

            case TutorialStatus.Settings:
                lines = new string[2] { "This is the pause menu. You can disconnect from the application here.", "You can enter the settings menu from here too! Click on the gear icon in the top of the panel." };
                objectives[0].text = "Press the gear button to open the settings menu";
                break;
        }

        //We call the DialogueScript
        dialogueScript.StartDialogue(lines);
    }


    private void EmptyObjectives()
    {
        //Objectives UI
        foreach(TMP_Text objective in objectives)
        {
            objective.text = "";
            objective.color = Color.black;
        }

        //Reset counter
       for(int i = 0; i < objectiveCounter.Length; i++)
        {
            objectiveCounter[i] = 0;
        }

    }

    public void CompleteObjective(int num)
    {
       
        switch (TutorialStatus)
        {
            case TutorialStatus.Movement:
                //Objectives
                objectives[num].color = Color.green;

                bool flag = true;

                //We check the flags
                foreach(TMP_Text objective in objectives)
                {
                    if (objective.color != Color.green) flag = false;
                }

                //If true, change tutorial
                if (flag) { TutorialStatus = TutorialStatus.Jumping; StartDialogue(); }
                
                break;

            case TutorialStatus.Jumping:
                //Objectives
                objectives[num].color = Color.green;

                TutorialStatus = TutorialStatus.Interaction; 
                StartDialogue();

                break;

            case TutorialStatus.Interaction:

                //Objectives

                ++objectiveCounter[0];
                objectives[num].text = "Press e with an interactable object." + objectiveCounter[0] + "/2";

                if (objectiveCounter[0] == 2) { 
                    objectives[num].color = Color.green;
                    TutorialStatus = TutorialStatus.Voice; 
                    StartDialogue();
                }

                break;

            case TutorialStatus.Voice:

                //Objectives
                ++objectiveCounter[0];
                objectives[num].text = "Press m to mute/unmute the chat" + objectiveCounter[0] + "/2";

                if (objectiveCounter[0] == 2)
                {
                    objectives[num].color = Color.green;
                    TutorialStatus = TutorialStatus.PreSettings;
                    StartDialogue();
                }

                break;

            case TutorialStatus.PreSettings:
                objectives[num].color = Color.green;
                fpsController.Deactivate();
                //Open Pause Menu
                pauseMenuSettings.Show();

                //SetDialogue

                TutorialStatus = TutorialStatus.Settings;
                StartDialogue();

                break;

            case TutorialStatus.Settings:
                objectives[num].color = Color.green;

                //SettingsTutorial Method
                
                OnSettingsTutorial();
                
                break;

            default:
                objectives[num].color = Color.green;
                break;
        }

    }


    private void OnSettingsTutorial()
    {
        settingsStatus++;

        switch (settingsStatus)
        {
            case SettingsStatus.Keys:
                {
                    moveTabs.ChangeToPanelKeys();

                    //string[] lines = new string[1] { "This is the Key menu. You can change the input keys from here." };

                    dialogueScript.StartDialogue();
                    break;
                }

            case SettingsStatus.Friends:
                {
                    moveTabs.ChangeToPanelFriends();

                    //string[] lines = new string[1] { "This is the Friends menu. You can see the friends that you add here." };

                    dialogueScript.StartDialogue();
                    break;
                }

            case SettingsStatus.Players:
                {
                    moveTabs.ChangeToPanelPlayer();
                    //string[] lines = new string[1] { "This is the Player menu. You can see the list of players here." };

                    dialogueScript.StartDialogue();
                    break;
                }

            case SettingsStatus.Finished:
                {

                    moveTabs.HideSettings();


                    //string[] lines = new string[2] { "That's all about the settings part.", "Now, go downstairs and interact with the podium to view a presentation" };


                    dialogueScript.StartDialogue();

                    gameManager.GameStatus = GameStatus.InGame;
                    gameManager.TutorialStatus = TutorialStatus.Presentation;
                    graphic.enabled = true;

                    triggerDetector.SetPresentationTutorial();


                    break;
                }
        }
    }





}
