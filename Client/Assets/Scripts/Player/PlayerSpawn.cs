using System.Collections;
using System.Collections.Generic;
using System.IO;
using ExitGames.Client.Photon;
using Newtonsoft.Json.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

public class PlayerSpawn : MonoBehaviourPunCallbacks, IOnEventCallback
{
    private Estados estado;

    private AudioController voiceChat;

    private Compressor compressor = new Compressor();

    public Animator animator;

    public GameObject[] playerPrefabs;

    public Transform[] spawnPoints;

    public GameObject Pausa;

    public GameObject chatManager;

    public GameObject Settings;

    public GameObject fileExplorer;

    public GameObject scope;

    public Camera presentationCamera = null;

    public GameObject playerCamera;

    public GameObject eventText;
    public bool onPresentationCamera = false;

    

    //Map Variables
    public string mapName;
    public TMP_Text loadingPressCanvas;

    bool escPul;

    bool TPul;

    bool LPul;

    //Static variables
    static bool reload = false;

    static Vector3 spawnPoint;

    //Player Character
    GameObject playerToSpawn;

    private void Start()
    {
        //States
        estado = Estados.Juego;
        escPul = false;
        TPul = false;

        

        chatManager.SetActive(true);

        //We check if it's the first time the user entered the room.
        if (reload == false)
        {
            int randomNumber = Random.Range(0, spawnPoints.Length);
            spawnPoint = spawnPoints[randomNumber].position;
        }

        voiceChat =
            GameObject.Find("VoiceManager").GetComponent<AudioController>();

        //Random avatar character
        if (
            PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] ==
            null ||
            (int) PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] ==
            6
        )
        {
            int value = Random.Range(0, 5);
            PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] = value;

            UnityEngine
                .Debug
                .Log(PhotonNetwork
                    .LocalPlayer
                    .CustomProperties["playerAvatar"] = value);
        }

        //Player instantation
        playerToSpawn =
            playerPrefabs[(int)
            PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];

        //CS script for movement activated
        playerToSpawn =
            (GameObject)
            PhotonNetwork
                .Instantiate(playerToSpawn.name,
                spawnPoint,
                Quaternion.identity);

        //-------------------------ACTIVATING CAM AND MOVEMENT ONLY ON LOCAL PLAYER------------------------//
        //this is because we only want the camera and the movement activated for the local player so by default the prefab have both cam and script deactivated. Here is where we activate it right in time, when everithing´s prepared.
        playerToSpawn.GetComponent<SC_FPSController>().enabled = true; //Camera of the player
        playerCamera =  playerToSpawn.transform.Find("PlayerCamera").gameObject;
        playerCamera.SetActive(true);
        //-------------------------ACTIVATING UI------------------------//
        //Prefab of the UI for VoiceChat
        playerToSpawn
            .transform
            .Find("PlayerUIPrefab")
            .gameObject
            .SetActive(true);

        //Scope

        scope = GameObject.Find("PlayerUIPrefab").transform.GetChild(1).gameObject;

        //UI for the Text Chat
        GameObject NamePlayerObject = GameObject.Find("NameUI"); //Find the canvas named NameUI(TMP text generate canvas and inside a tmp text)
        playerToSpawn.transform.Find("NameUI").gameObject.SetActive(true); //We activate the hole canvas
        NamePlayerObject
            .GetComponent<PlayerNameDisplay>()
            .SetPlayerName(playerToSpawn
                .GetComponent<PhotonView>()
                .Owner
                .NickName); //Getting the PlayerNameDisplay component(script controller) we call the setName method we defined it just change the text value of a variable that corresponds to the text object inside de canvas


        //-------------------------ACTIVATING UI END------------------------//
        animator = playerToSpawn.transform.GetChild(0).GetComponent<Animator>();
        voiceChat.CheckMicroImage();
        PhotonNetwork.IsMessageQueueRunning = true;
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    private void Update()
    {
        //Set UI PlayerName
        GameObject[] playersInGame =
            GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in playersInGame)
        {
            if (
                player
                    .transform
                    .GetChild(3)
                    .GetChild(0)
                    .GetComponent<TMP_Text>()
                    .text ==
                "Name"
            )
            {
                Debug
                    .Log("There is someone named: " +
                    player.GetComponent<PhotonView>().Owner.NickName +
                    " in the game!");
                player
                    .transform
                    .GetChild(3)
                    .GetComponent<PlayerNameDisplay>()
                    .SetPlayerName(player
                        .GetComponent<PhotonView>()
                        .Owner
                        .NickName);
            }

            if (!player.gameObject.GetComponent<PhotonView>().IsMine) player.transform.GetChild(2).gameObject.SetActive(false);
        }
        if (estado == Estados.Juego)
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && !escPul)
            {
                //Deactivate presentation text
                  if(eventText != null) eventText.SetActive(false);
                //Deactivate scope
                scope.SetActive(false);
                //Start Animator
                animator.speed = 0;
                object[] content =
                    new object[] {
                        playerToSpawn.GetComponent<PhotonView>().Owner.NickName,
                        animator.speed,
                        "Stop&Replay"
                    };
                RaiseEventOptions raiseEventOptions =
                    new RaiseEventOptions { Receivers = ReceiverGroup.Others }; // You would have to set the Receivers to All in order to receive this event on the local client as well
                PhotonNetwork
                    .RaiseEvent(2,
                    content,
                    raiseEventOptions,
                    SendOptions.SendReliable);

                Pausa.SetActive(true);

                //Time.timeScale = 0;
                playerToSpawn.GetComponent<SC_FPSController>().enabled = false;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None; // Desactiva el bloqueo cursor
                estado = Estados.Pausa;
                escPul = true; //Escape activado
                UnityEngine.Debug.Log (estado);
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.T) && !TPul)
            {
                //Start Animation
                animator.speed = 0;
                object[] content =
                    new object[] {
                        playerToSpawn.GetComponent<PhotonView>().Owner.NickName,
                        animator.speed,
                        "Stop&Replay"
                    };
                RaiseEventOptions raiseEventOptions =
                    new RaiseEventOptions { Receivers = ReceiverGroup.Others }; // You would have to set the Receivers to All in order to receive this event on the local client as well
                PhotonNetwork
                    .RaiseEvent(2,
                    content,
                    raiseEventOptions,
                    SendOptions.SendReliable);

                UnityEngine.Debug.Log("T pulsada");
                chatManager.SetActive(true);
                chatManager
                    .GetComponent<PhotonChatManager>()
                    .ChatConnectOnClick();
                playerToSpawn.GetComponent<SC_FPSController>().enabled = false;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None; // Desactiva el bloqueo cursor
                estado = Estados.Pausa;
                TPul = true; //Escape activado
                UnityEngine.Debug.Log (estado);
            }

        
            if(UnityEngine.Input.GetKeyDown(KeyCode.K) && presentationCamera != null){

                if(onPresentationCamera){
                    presentationCamera.enabled = false;
                    playerCamera.SetActive(true);
                      eventText.SetActive(true);
                }

                else{
                     presentationCamera.enabled = true;
                    playerCamera.SetActive(false);
                    eventText.SetActive(false);
                }

                onPresentationCamera = !onPresentationCamera;



            }
        }

        if (!UnityEngine.Input.GetKeyDown(KeyCode.Escape)) escPul = false; // Detecta si no está pulsado

        //Game State
        if (estado == Estados.Pausa)
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && !escPul)
            {
                //Activate scope
                 scope.SetActive(true);
                 //Activate presentation text
                 if(eventText != null) eventText.SetActive(true);
                
                //Stop Animation
                animator.speed = 1;
                object[] content =
                    new object[] {
                        playerToSpawn.GetComponent<PhotonView>().Owner.NickName,
                        animator.speed,
                        "Stop&Replay"
                    };
                RaiseEventOptions raiseEventOptions =
                    new RaiseEventOptions { Receivers = ReceiverGroup.Others }; // You would have to set the Receivers to All in order to receive this event on the local client as well
                PhotonNetwork
                    .RaiseEvent(2,
                    content,
                    raiseEventOptions,
                    SendOptions.SendReliable);

                //Activate Settings Window and stop
                Settings.SetActive(false);
                Pausa.SetActive(false);
                chatManager.SetActive(false);
                TPul = false;
                Time.timeScale = 1;
                playerToSpawn.GetComponent<SC_FPSController>().enabled = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked; // Menu de opciones, para que se bloquee la camara
                estado = Estados.Juego;
                UnityEngine.Debug.Log (estado);
            }
        }

    }

    public enum Estados
    {
        Juego,
        Pausa
    }

    //Recibir eventos
    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            //New user
            case 1:
                {
                    PhotonNetwork.IsMessageQueueRunning = false;

                    //We maintain the same state between reloads.
                    reload = true;
                    spawnPoint = playerToSpawn.transform.position;

                    //We reload the level
                    PhotonNetwork.LoadLevel (mapName);
                    break;
                }
            //Change animator speed
            case 2:
                {
                    object[] data = (object[]) photonEvent.CustomData;

                    GameObject[] playersInGame =
                        GameObject.FindGameObjectsWithTag("Player");
                    foreach (GameObject player in playersInGame)
                    {
                        if (
                            player.GetComponent<PhotonView>().Owner.NickName ==
                            (string) data[0]
                        )
                        {
                            switch ((string) data[2]){

                                case "Walking":
                                {
                                    player
                                        .transform
                                        .GetChild(0)
                                        .GetComponent<Animator>()
                                        .SetBool("Walking",(bool) data[1]);
                                    break;
                                }
                                case "Running":
                                {
                                    player
                                        .transform
                                        .GetChild(0)
                                        .GetComponent<Animator>()
                                        .SetBool("Running",(bool) data[1]);
                                    break;
                                }
                                case "Stop&Replay":
                                {
                                    player
                                        .transform
                                        .GetChild(0)
                                        .GetComponent<Animator>()
                                        .speed = (float) data[1];
                                    break;
                                }
                                case "SpeedAnim":
                                {
                                    player
                                        .transform
                                        .GetChild(0)
                                        .GetComponent<Animator>()
                                        .SetFloat("Speed",(float) data[1]);
                                    break;
                                }
                            }
                        }
                    }
                    break;
                }
            //Light Event
            case 21:
                {
                    object[] data = (object[]) photonEvent.CustomData;

                    GameObject eventObject = GameObject.Find((string) data[0]);

                    eventObject.GetComponent<Lamp>().activate(false);
                    break;
                }
            //Event FileExplorer(GET)
            case 22:
                {
                    loadingPressCanvas.enabled = true;
                    loadingPressCanvas.SetText("Loading");
                    object[] data = (object[]) photonEvent.CustomData;
                    fileExplorer
                        .GetComponent<FileExplorer>()
                        .downloadImages((string) data[0]);
                    break;
                }
            //Event to move slide
            case 23:
                {
                    object[] data = (object[]) photonEvent.CustomData;

                    if ((string) data[0] == "Back")
                    {
                        //Back presentation
                        GameObject eventObject = GameObject.Find("Back");
                        eventObject
                            .GetComponent<BackPresentation>()
                            .activate(false);
                    }
                    else
                    {
                        //Advance presentation
                        GameObject eventObject = GameObject.Find("Advance");
                        eventObject
                            .GetComponent<AdvancePresentation>()
                            .activate(false);
                    }
                    break;
                }
            //Event FileExplorer Video
            case 24:
                {
                    loadingPressCanvas.enabled = true;
                    loadingPressCanvas.SetText("Loading");
                    object[] data = (object[]) photonEvent.CustomData;
                    fileExplorer
                        .GetComponent<FileExplorer>()
                        .SetVideo((string) data[0],(string) data[1],compressor.Decompress((byte[]) data[2]));
                    break;
                }
            //Event FileExplorer Image
            case 25:
                {
                    loadingPressCanvas.enabled = true;
                    loadingPressCanvas.SetText("Loading");
                    object[] data = (object[]) photonEvent.CustomData;
                    fileExplorer
                        .GetComponent<FileExplorer>()
                        .SetImage((byte[]) data[0]);
                    break;
                }
        }
    }

    //Function to set the Presentation Camera on Room Entered

   public void setPresentationCamera(Camera camera) {

        if(camera == null)
        {
            //We deactivate the camera
            presentationCamera = null;

            //We activate UI

            eventText = GameObject.Find("PlayerUIPrefab").transform.GetChild(3).gameObject;
            eventText.SetActive(false);
            eventText = null;

        }

        else{


       
            //We obtain the camera
            presentationCamera = camera;

            //We activate UI

            eventText = GameObject.Find("PlayerUIPrefab").transform.GetChild(3).gameObject;

            eventText.SetActive(true);
        }
            

        
    }

    

    
    //Recibir eventos
    public void ChangeRoom(string map)
    {
        if (map == "Mapa2")
        {
            //We reload the level
            PhotonNetwork.LoadLevel (map);
        }
    }
}
