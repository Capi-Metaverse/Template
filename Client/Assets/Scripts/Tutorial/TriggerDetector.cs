using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static System.Windows.Forms.LinkLabel;


public class TriggerDetector : MonoBehaviour
{

    [SerializeField] private Canvas canvasDialogue;
    private Dialogue dialogueScript;
    private GameManagerTutorial gameManager;

    //Set flags dictionary
    private Dictionary<string, bool> flags = new Dictionary<string, bool>();

    public TMP_Text objective1;
    public TMP_Text objective2;
    public TMP_Text objective3;
    public TMP_Text objective4;

    public SC_FPSController fPSController;

    private int interaction = 0;

    public TMP_Text tutorialNumber;

    private int arrowLeftCounter, arrowRightCounter, kPressedCounter = 0;

    public void Start()
    {
        gameManager = GameObject.Find("Manager").GetComponent<GameManagerTutorial>();
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
            switch (gameManager.TutorialStatus)
            {
                case TutorialStatus.Movement:
                    {
                        if (Input.GetKey("w")) { flags["W"] = true; objective1.color = Color.green; }
                        if (Input.GetKey("a"))
                        {
                            flags["A"] = true; objective3.color = Color.green;
                        }
                        if (Input.GetKey("s"))
                        {
                            flags["S"] = true; objective2.color = Color.green;
                        }
                        if (Input.GetKey("d")) { flags["D"] = true; objective4.color = Color.green; }

                        if (flags["W"] && flags["A"] && flags["S"] && flags["D"])
                        {
                            objective1.text = "";
                            objective1.color = Color.black;
                            objective1.text = "Press space to jump.";
                            tutorialNumber.text = "2";
                            objective2.text = "";
                            objective3.text = "";
                            objective4.text = "";
                            RestartDialogue(TutorialStatus.Jumping, new string[2] { "Congratulations! In the metaverse you can jump too.", "Press Space to Jump" });
                           
                        }
                        break;
                    }
                case TutorialStatus.Jumping:
                    {
                        if (!this.gameObject.GetComponent<CharacterController>().isGrounded)
                        {
                            flags["Space"] = true;
                            objective1.color = Color.green;
                          



                        }
                        else
                        {
                            if (flags["Space"] && this.gameObject.GetComponent<CharacterController>().isGrounded)
                            {
                                RestartDialogue(TutorialStatus.Interaction, new string[2] { "Well Done! Now, let's do something more interactive.", "Go near a lamp and press e to interact with it" });
                                objective1.text = "";
                                objective1.color = Color.black;
                                tutorialNumber.text = "3";
                                objective1.text = "Press e with an interactable object. 0/2";
                            }
                        }
                        break;
                    }
              
                case TutorialStatus.Voice:
                    {
                        if (Input.GetKey("m"))
                        {
                            flags["M"] = true;
                            objective1.color = Color.green;
                            RestartDialogue(TutorialStatus.PreSettings, new string[2] { "After turning the lights on/off, it's time to see how to modify the settings", "Press ESC to show the Pause and Settings Menus" });
                            objective1.text = "";
                            tutorialNumber.text = "5";
                            objective1.color = Color.black;
                            objective1.text = "Press Escape to open the pause menu";
                        };
                        break;
                    }
                case TutorialStatus.PreSettings:
                    {
                        if (Input.GetKey("escape"))
                        {
                            flags["ESC"] = true;
                             
                            //RestartDialogue(TutorialStatus.Presentation, new string[2] { "Hello again again again again again", "Go upstairs and interact with the podium to view a presentation" });
                        };
                        break;
                    }
                case TutorialStatus.Presentation:
                    {
                    

                        if (Input.GetKey("k"))
                        {
                            flags["K"] = true;
                            RestartDialogue(TutorialStatus.Animations, new string[2] { "Now, let's do a special move. ", "Approach a mirror, press B to show the Animation Roulette and select a move" });
                        };
                        break;
                    }
                case TutorialStatus.Animations:
                    {
                        if (Input.GetKey("b"))
                        {
                            flags["B"] = true;
                            RestartDialogue(TutorialStatus.Finished, new string[2] { "Congratulations! You've finished the tutorial. Now you're free to continue practicing in the tutorial or to change to other maps.", "Press C when you're ready to change" });
                        };
                        break;
                    }
                case TutorialStatus.Finished:
                    {
                        if (Input.GetKey("c"))
                        {
                            flags["C"] = true;
                            Debug.Log("Exiting");
                            //Close 
                        };
                        break;
                    }
            }

        }
    }


    public void endInteraction()
    {
        if(gameManager.TutorialStatus == TutorialStatus.Interaction)
        {
            ++interaction;
            Debug.Log(interaction);
            objective1.text = "Press e with an interactable object." + interaction + "/2";
            if (interaction == 2)
            {
                flags["E"] = true;
                objective1.color = Color.green;
                RestartDialogue(TutorialStatus.Voice, new string[2] { "Now, let's see how to talk with someone.", "Press M to mute and unmute your voice" });
                objective1.text = "";
                objective1.color = Color.black;
                tutorialNumber.text = "4";
                objective1.text = "Press m to activate the chat.";
            }
        }

    }

    public void SetPresentationTutorial()
    {
        objective1.text = "";
        tutorialNumber.text = "6";
        objective1.color = Color.black;
        objective2.color = Color.black;
        objective3.color = Color.black;
        objective1.text = "Click the right arrow 0/2";
        objective2.text = "Click the left arrow 0/2";
        objective3.text = "Click the K key in a presentation area 0/2";
    }

    public void OnLeftArrow()
    {
        if(arrowLeftCounter < 2)
        {
            ++arrowLeftCounter;
            objective2.text = "Click the left arrow." + arrowLeftCounter + "/2";

            if(arrowLeftCounter == 2)
            {
                objective2.color = Color.green;

                if((arrowRightCounter == 2) && (kPressedCounter == 2))
                {
                    SetAnimationTutorial();
                }
            }

       
        }

    }

    public void OnRightArrow()
    {
        if (arrowRightCounter < 2)
        {
            ++arrowRightCounter;
            objective1.text = "Click the right arrow." + arrowRightCounter + "/2";

            if (arrowRightCounter == 2)
            {
                objective1.color = Color.green;

                if ((arrowLeftCounter == 2) && (kPressedCounter == 2))
                {
                    SetAnimationTutorial();
                }
            }
        }

    }

    public void OnPresentation()
    {
        if (kPressedCounter < 2)
        {
            ++kPressedCounter;
            objective3.text = "Click the K key in a presentation area." + kPressedCounter + "/2";

            if (kPressedCounter == 2)
            {
                objective3.color = Color.green;

                if ((arrowLeftCounter == 2) && (arrowRightCounter == 2))
                {
                    SetAnimationTutorial();
                }
            }
        }
    }

    public void SetAnimationTutorial()
    {
        flags["K"] = true;
        RestartDialogue(TutorialStatus.Animations, new string[2] { "Now, let's do a special move. ", "Approach a mirror, press B to show the Animation Roulette and select a move" });
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
        fPSController.EventWheelController();
        RestartDialogue(TutorialStatus.Finished, new string[2] { "Congratulations! You've finished the tutorial. Now you're free to continue practicing in the tutorial or to change to other maps.", "Press C when you're ready to change" });
        StartCoroutine(CancelAnimation());
    }

    IEnumerator CancelAnimation()
    {
        yield return new WaitForSeconds(2);
        fPSController.animator.SetInteger("AnimationWheel", (int)AnimationList.None);
    }

    public void RestartDialogue(TutorialStatus tutorialStatusValue, string[] lines)
    {
        gameManager.TutorialStatus = tutorialStatusValue;
        gameManager.DialogueStatus = DialogueStatus.InDialogue;
        fPSController.playerUI.EventTextOff();
        dialogueScript.lines = lines;
        dialogueScript.textComponent.text = string.Empty;
        dialogueScript.EnableDialogue();

        dialogueScript.StartDialogue();
    }

}
