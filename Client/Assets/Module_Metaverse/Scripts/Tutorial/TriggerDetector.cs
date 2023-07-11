
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cursor = UnityEngine.Cursor;

namespace Tutorial
{
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

        public TMP_Text tutorialNumber;

        //EventWheel
        public GameObject emoteWheel;
        public bool isEventWheelOpen = false;
        public Animator animator;

        /// <summary>
        /// Gets components and defines flags.
        /// </summary>
        public void Start()
        {
            gameManager = GameObject.Find("ManagerTutorial").GetComponent<GameManagerTutorial>();
            dialogueScript = canvasDialogue.GetComponentInChildren<Dialogue>();

            //Set dictionary initial values
            flags.Add("Space", false);
            flags.Add("C", false);
        }

        /// <summary>
        /// Detects if keys are being pressed in certain states.
        /// </summary>
        public void Update()
        {
            if (gameManager.DialogueStatus == DialogueStatus.InGame && gameManager.GameStatus == GameStatus.InGame)
            {
                if (Input.GetKeyDown("j"))
                {
                    gameManager.StartDialogue(true);
                }

                if (Input.GetKey("c"))
                {
                    flags["C"] = true;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;

                    //Add to playfab
                    var request = new UpdateUserDataRequest
                    {
                        Data = new Dictionary<string, string>
                    {
                        {"NewUser", "false"}
                    }
                    };
                    PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
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
                                gameManager.CompleteObjective(0);
                            };
                            break;
                        }
                    case TutorialStatus.Finished:
                        {

                            if (Input.GetKeyDown("b"))
                            {
                                gameManager.EventWheelController();
                            };

                            if (Input.GetKey("c"))
                            {
                                flags["C"] = true;
                                Cursor.lockState = CursorLockMode.None;
                                Cursor.visible = true;
                                SceneManager.LoadScene("LoginPlayFab_Module");
                                //Close 
                            };
                            break;
                        }
                }
            }
        }


        /// <summary>
        /// Completes Presentation Objective 0. Is activated when the presentation left arrow is pressed.
        /// </summary>
        public void OnLeftArrow()
        {
            gameManager.CompleteObjective(0);

        }

        /// <summary>
        /// Completes Presentation Objective 1. Is activated when the presentation right arrow is pressed.
        /// </summary>
        public void OnRightArrow()
        {
            gameManager.CompleteObjective(1);

        }

        /// <summary>
        /// Completes the Presentation Objective 2. Is activated when there is a change between cameras.
        /// </summary>
        public void OnPresentation()
        {
            gameManager.CompleteObjective(2);
        }

        /// <summary>
        /// PlayFab. Shows the error produced when sending data to server.
        /// </summary>
        /// <param name="obj"></param>
        public void OnError(PlayFabError obj)
        {
            Debug.Log("[PlayFab] Error");
        }

        /// <summary>
        /// PlayFab. Changes the scene. Is activated when the data was sent succesfully.
        /// </summary>
        /// <param name="obj"></param>
        public void OnDataSend(UpdateUserDataResult obj)
        {
            Debug.Log("[PlayFab] Data Sent");

            if (gameManager.TutorialStatus != TutorialStatus.Finished)
            {
                SceneManager.LoadScene("1.Start");
            }
        }
    }

}
