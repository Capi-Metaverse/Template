using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static System.Windows.Forms.LinkLabel;

public enum TutorialStatus
{
    Movement,
    Jumping,
    Interaction,
    Voice,
    Settings,
    Presentation,
    Animations,
    Finished
}

public class TriggerDetector : MonoBehaviour
{

    [SerializeField]private Canvas canvasDialogue;
    private Dialogue dialogueScript;

    private TutorialStatus tutorialStatus = TutorialStatus.Movement;

    public TutorialStatus TutorialStatus { get => tutorialStatus; set => tutorialStatus = value; }

    //Set flags dictionary
    private Dictionary<string,bool> flags = new Dictionary<string,bool>();
    
    public void Start()
    {
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
        if (dialogueScript.DialogueStatus == DialogueStatus.InGame)
        {
            switch (tutorialStatus)
            {
                case TutorialStatus.Movement:
                    {
                        if (Input.GetKey("w")) flags["W"] = true;
                        if (Input.GetKey("a")) flags["A"] = true;
                        if (Input.GetKey("s")) flags["S"] = true;
                        if (Input.GetKey("d")) flags["D"] = true;

                        if (flags["W"] && flags["A"] && flags["S"] && flags["D"])
                        {
                            RestartDialogue(TutorialStatus.Jumping,new string[2] { "Hello again", "Press Space to Jump" });
                        }
                        break;
                    }
                case TutorialStatus.Jumping:
                    {
                        if (Input.GetKey("space")) {
                            flags["Space"] = true;
                            RestartDialogue(TutorialStatus.Interaction, new string[2] { "Hello again again", "Go near a lamp and press e to interact with it" });
                        };
                        break;
                    }
                case TutorialStatus.Interaction:
                    {
                        if (Input.GetKey("e"))
                        {
                            flags["E"] = true;
                            RestartDialogue(TutorialStatus.Voice, new string[2] { "Hello again again again", "Press M to mute and unmute the voice chat" });
                        };
                        break;
                    }
                case TutorialStatus.Voice:
                    {
                        if (Input.GetKey("m"))
                        {
                            flags["M"] = true;
                            RestartDialogue(TutorialStatus.Settings, new string[2] { "Hello again again again again", "Press ESC to show the Pause and Settings Menus" });
                        };
                        break;
                    }
                case TutorialStatus.Settings:
                    {
                        if (Input.GetKey("escape"))
                        {
                            flags["ESC"] = true;
                            RestartDialogue(TutorialStatus.Presentation, new string[2] { "Hello again again again again again", "Go upstairs and interact with the podium to view a presentation" });
                        };
                        break;
                    }
                case TutorialStatus.Presentation:
                    {
                        if (Input.GetKey("k"))
                        {
                            flags["K"] = true;
                            RestartDialogue(TutorialStatus.Animations, new string[2] { "Hello again again again again again", "Press B to show the Animation Roulette and click one" });
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

    public void RestartDialogue(TutorialStatus tutorialStatusValue,string[] lines)
    {
        tutorialStatus = tutorialStatusValue;
        dialogueScript.DialogueStatus = DialogueStatus.InDialogue;

        dialogueScript.lines = lines;
        dialogueScript.textComponent.text = string.Empty;
        dialogueScript.gameObject.SetActive(true);
        dialogueScript.StartDialogue();
    }
    
}
