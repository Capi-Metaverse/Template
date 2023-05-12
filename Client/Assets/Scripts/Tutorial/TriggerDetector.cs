
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Windows.Forms.LinkLabel;
using Cursor = UnityEngine.Cursor;

public class TriggerDetector : MonoBehaviour
{

    [SerializeField] private Canvas canvasDialogue;
    [SerializeField] private PlayerUI playerUI;
    private Dialogue dialogueScript;
    private GameManagerTutorial gameManager;

    //Set flags dictionary
    private Dictionary<string, bool> flags = new Dictionary<string, bool>();

    public TMP_Text objective1;
    public TMP_Text objective2;
    public TMP_Text objective3;
    public TMP_Text objective4;

    public TMP_Text outText;

    public SC_FPSController fPSController;

    private int interaction = 0;

    public TMP_Text tutorialNumber;

    private int arrowLeftCounter, arrowRightCounter, kPressedCounter, mPressedCounter = 0;

    //EventWheel
    public GameObject emoteWheel;
    public bool isEventWheelOpen = false;
    public Animator animator;

    public void Start()
    {
        gameManager = GameObject.Find("ManagerTutorial").GetComponent<GameManagerTutorial>();
        dialogueScript = canvasDialogue.GetComponentInChildren<Dialogue>();

        //Set dictionary initial values
        flags.Add("W", false);
        flags.Add("A", false);
        flags.Add("S", false);
        flags.Add("D", false);
        flags.Add("Space", false);
        flags.Add("E", false);
        flags.Add("M", false);
        flags.Add("ESC", false);
        flags.Add("K", false); //Change key
        flags.Add("B", false);
        flags.Add("C", false);
    }
    public void Update()
    {
        if (gameManager.DialogueStatus == DialogueStatus.InGame && gameManager.GameStatus == GameStatus.InGame)
        {
            if (Input.GetKeyDown("j"))
            {
                //ReShowDialogue();
            }
            if (Input.GetKey("c"))
            {
                flags["C"] = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                var request = new UpdateUserDataRequest
                {
                    Data = new Dictionary<string, string>
            {
                {"NewUser", "false"}
            }
                };
                PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);


               


                //Close 
            };
            switch (gameManager.TutorialStatus)
            {
                case TutorialStatus.Movement:
                    {
                        if (Input.GetKeyDown("w")) gameManager.CompleteObjective(0);
                        if (Input.GetKeyDown("s")) gameManager.CompleteObjective(1);
                        if (Input.GetKeyDown("a")) gameManager.CompleteObjective(2);
                        if (Input.GetKeyDown("d")) gameManager.CompleteObjective(3);
                        break;
                    }
                    
                case TutorialStatus.Jumping:
                    {
                        if (Input.GetKeyDown(KeyCode.Space)) flags["Space"] = true;

                        else
                        {
                            if (flags["Space"] && this.gameObject.GetComponent<CharacterController>().isGrounded) gameManager.CompleteObjective(0);
                        }

                        break;
                    }
                   
                case TutorialStatus.Voice:
                    {
                        if (Input.GetKeyDown("m")) gameManager.CompleteObjective(0);
                        break;

                    }

                case TutorialStatus.PreSettings:
                    {
                        if (Input.GetKeyDown(KeyCode.Escape)) gameManager.CompleteObjective(0);
                        break;
                    }

                     case TutorialStatus.Animations:
                         {
                             if (Input.GetKeyDown("b"))
                             {
                                 EventWheelController();
                             };
                             break;
                         }
                     case TutorialStatus.Finished:
                         {

                             if (Input.GetKeyDown("b"))
                             {
                                 EventWheelController();
                             };

                             if (Input.GetKey("c"))
                             {
                                 flags["C"] = true;
                                 Cursor.lockState = CursorLockMode.None;
                                 Cursor.visible = true;
                                 SceneManager.LoadScene("1.Start");
                                 //Close 
                             };
                             break;
                         }
            }

        }
        }



    public void OnLeftArrow()
    {
       gameManager.CompleteObjective(0);

    }

    public void OnRightArrow()
    {
        gameManager.CompleteObjective(1);

    }

    public void OnPresentation()
    {
        gameManager.CompleteObjective(2);
    }


    public void EventWheelController()
    {
        if (isEventWheelOpen)
        {
            fPSController.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            playerUI.ShowUI();
            emoteWheel.SetActive(false);
            isEventWheelOpen = false;
        }
        else
        {
            fPSController.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            playerUI.HideUI();
            emoteWheel.SetActive(true);
            isEventWheelOpen = true;
        }
    }

    public void SetAnimationTutorial()
    {
        flags["K"] = true;
        playerUI.PresentationTextOff();
        //RestartDialogue(TutorialStatus.Animations, new string[2] { "Now, let's do a special move. ", "Approach a mirror, press B to show the Animation Roulette and select a move" });
        objective1.text = "";
        objective2.text = "";
        objective3.text = "";
        objective1.color = Color.black;
        tutorialNumber.text = "7";
        objective1.text = "Press B to play an animation.";
    }

    public void EndAnimationTutorial()
    {
        flags["B"] = true;
        EventWheelController();

        objective1.text = "";
        objective1.color = Color.black;
        tutorialNumber.text = "8";
        //RestartDialogue(TutorialStatus.Finished, new string[2] { "Congratulations! You've finished the tutorial. Now you're free to continue practicing in the tutorial or to change to other maps.", "Press C when you're ready to change" });
        StartCoroutine(CancelAnimation());
        outText.enabled = true;


        //Add to playfab

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"NewUser", "false"}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);


    }


    public void OnError(PlayFabError obj)
    {
        Debug.Log("[PlayFab-ManageData] Error");
    }

    public void OnDataSend(UpdateUserDataResult obj)
    {
        Debug.Log("[PlayFab-ManageData] Data Sent");

        if(gameManager.TutorialStatus != TutorialStatus.Finished)
        {
            SceneManager.LoadScene("1.Start");
        }
    }

    IEnumerator CancelAnimation()
    {
        yield return new WaitForSeconds(2);
        fPSController.animator.SetInteger("AnimationWheel", (int)AnimationList.None);
    }
    /*
  

    public void ReShowDialogue()
    {
        fPSController.playerUI.EventTextOff();
        dialogueScript.textComponent.text = string.Empty;
        dialogueScript.EnableDialogue();

        dialogueScript.StartDialogue();
    }
    */
}
