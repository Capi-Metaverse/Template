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

/*AT FIRST THIS CLASS WAS ONLY TO MAKE THE PLAYERS SPAWN ON THE MAP BUT BY NOW IT 
HANDLES ALMOST EVERITHING THAT HAS SOMETHING TO DO WITH THE PLAYER(EVENTS, INTERACTIONS ETC...)
*/
public class PlayerSpawn : MonoBehaviourPunCallbacks, IOnEventCallback
{
    InputManager inputManager;
    
    /*------------MANAGERS AS GAMEOBJECTS IN SCENE MAP----------------*/
    private AudioController voiceChat;//Manager for the voiceChat, not in scene object
    public Animator animator;//Animation manager
    public GameObject Pausa;//Pausa is an object in scene map, you can see it as the manager of the pause state
    public GameObject chatManager;//Also in scene map is the manager for the TextChat(T to open TextChat)
    public GameObject Settings;//The same as Pausa but for settings, the state will be Pausa too cause the setting are accesible from Pausa
    public GameObject fileExplorer;//Object in scene map, is the manager for the FileExploring system
    public Disconnect disconnect;

    /*--------------------BOOLEANS FOR CONDITIONAL CHECKING---------------------*/
    private Estados estado; //With this we keep track of the current state so we can use it in conditionals. States are (Game, Pause)
    public bool onPresentationCamera = false;//Boolean to know if pressMode is on or not
    static bool reload = false;//For spawn when you fall of the map
    bool escPul;//Reference if ESC key is pushed or not(ESC opens the Menu and you´ll be on Pause State)
    bool TPul;//Reference if T key is pushed or not(T opens the TextChat)
    bool LPul;//Reference if L key is pushed or not(For loading/Uploading items, DEPRECATED)

    /*-------------UTILITY VARIABLES-----------------*/
    public string mapName;
    public GameObject[] playerPrefabs;//This are the player models(ReadyPlayerMe), all of them in a list
    public Camera presentationCamera = null;//Outside of the PlayerPrefab. With K you can change from playerCam to this cam that look ortographically to the presentation
    public GameObject playerCamera;//Inside of the PlayerPrefab
    public TMP_Text loadingPressCanvas;//Here is where the presentation images appear
    GameObject playerToSpawn;//Player Character
    public Transform[] spawnPoints;//Array with points corresponding to points in the map, the players spawn randomly
    static Vector3 spawnPoint;//A 3 coordinate point where the player must spawn
    public GameObject scope;//Actually this is the crosshair in game
    public GameObject eventText;//A text for displaying when an event happens
    private Compressor compressor = new Compressor();//Class used to compress video when presenting
    
    public enum Estados
    {
        Juego,Pausa
    }
    
    /*-------------METHODS-----------------*/
     private void Start()
    {
        inputManager = GameObject.FindObjectOfType<InputManager>();
        //We activate the message Queue again
        PhotonNetwork.IsMessageQueueRunning = true;
    }

    public override void OnJoinedRoom()
    {
        //We are connected now inside of a room, so we instantiate everything
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
        voiceChat = GameObject.Find("VoiceManager").GetComponent<AudioController>();

        //Random avatar character, 6 is the random character
        if (PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == null || (int) PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == 6)
        {
            int value = Random.Range(0, 5);
            PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] = value;

            UnityEngine.Debug.Log(PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] = value);
        }

        //Player instantation
        playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];

        //CS script for movement activated
        playerToSpawn =(GameObject)PhotonNetwork.Instantiate(playerToSpawn.name,spawnPoint,Quaternion.identity);

        //-------------------------ACTIVATING CAM AND MOVEMENT ONLY ON LOCAL PLAYER------------------------//
        //this is because we only want the camera and the movement activated for the local player so by default the prefab have both cam and script deactivated. Here is where we activate it right in time, when everithing´s prepared.
        playerToSpawn.GetComponent<SC_FPSController>().enabled = true; //Camera of the player
        playerCamera =  playerToSpawn.transform.Find("PlayerCamera").gameObject;
        playerCamera.SetActive(true);

        //-------------------------ACTIVATING UI------------------------//
        playerToSpawn.transform.Find("PlayerUIPrefab").gameObject.SetActive(true);//Prefab of the UI for VoiceChat
        scope = GameObject.Find("PlayerUIPrefab").transform.GetChild(1).gameObject;//Scope


        //Añadir botones
        PlayerUiPrefab playerUiprefab = GameObject.Find("PlayerUIPrefab").GetComponent<PlayerUiPrefab>();
        playerUiprefab.ChangeLetter(inputManager.GetKeyNameForButton("Interact"));
        playerUiprefab.ChangeLetterK(inputManager.GetKeyNameForButton("ChangeCamera"));

        //UI for the name displaying above the head of the players
        GameObject NamePlayerObject = GameObject.Find("NameUI"); //Find the canvas named NameUI(TMP text generate canvas and inside a tmp text)
        playerToSpawn.transform.Find("NameUI").gameObject.SetActive(true); //We activate the hole canvas
        NamePlayerObject.GetComponent<PlayerNameDisplay>().SetPlayerName(playerToSpawn.GetComponent<PhotonView>().Owner.NickName); //Getting the PlayerNameDisplay component(script controller) we call the setName method we defined it just change the text value of a variable that corresponds to the text object inside de canvas

        //-------------------------ACTIVATING UI END------------------------//
        animator = playerToSpawn.transform.GetChild(0).GetComponent<Animator>();
        voiceChat.CheckMicroImage();
        PhotonNetwork.IsMessageQueueRunning = true;//What is this????????
    }

    //When you enter this override function means you just call leaveRoom so the game loads the previous scene which is the lobby
    public override void OnConnectedToMaster()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    private void Update()
    {
        //Set UI PlayerName
        GameObject[] playersInGame = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in playersInGame)
        {
            if (player.transform.GetChild(3).GetChild(0).GetComponent<TMP_Text>().text =="Name")
            {
                Debug.Log("There is someone named: " + player.GetComponent<PhotonView>().Owner.NickName + " in the game!");player.transform.GetChild(3).GetComponent<PlayerNameDisplay>().SetPlayerName(player.GetComponent<PhotonView>().Owner.NickName);
            }

            if (!player.gameObject.GetComponent<PhotonView>().IsMine) 
            {
                player.transform.GetChild(2).gameObject.SetActive(false);
            }  
        }

        if (estado == Estados.Juego)
        {
            //ESC key down(PauseMenu)
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && !escPul)
            {
                //Deactivate presentation text
                if(eventText != null) 
                {
                    eventText.SetActive(false);
                }
                
                //Deactivate scope
                scope.SetActive(false);
                //Start Animator
                animator.speed = 0;
                object[] content =new object[] {playerToSpawn.GetComponent<PhotonView>().Owner.NickName,animator.speed,"Stop&Replay"};
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others }; // You would have to set the Receivers to All in order to receive this event on the local client as well
                PhotonNetwork.RaiseEvent(2,content,raiseEventOptions,SendOptions.SendReliable);

                Pausa.SetActive(true);
                playerToSpawn.GetComponent<SC_FPSController>().enabled = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None; // Desactiva el bloqueo cursor
                estado = Estados.Pausa;
                escPul = true; //Escape activado
                UnityEngine.Debug.Log (estado);
            }

            //T key down(TextChat)
            if (UnityEngine.Input.GetKeyDown(KeyCode.T) && !TPul)
            {
                //Start Animation
                animator.speed = 0;
                object[] content =new object[] {playerToSpawn.GetComponent<PhotonView>().Owner.NickName,animator.speed,"Stop&Replay"};
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others }; // You would have to set the Receivers to All in order to receive this event on the local client as well
                PhotonNetwork.RaiseEvent(2,content,raiseEventOptions,SendOptions.SendReliable);

                UnityEngine.Debug.Log("T pulsada");
                chatManager.SetActive(true);
                chatManager.GetComponent<PhotonChatManager>().ChatConnectOnClick();
                playerToSpawn.GetComponent<SC_FPSController>().enabled = false;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None; // Desactiva el bloqueo cursor
                estado = Estados.Pausa;
                TPul = true; //Escape is activated, true
                UnityEngine.Debug.Log (estado);
            }

            //K key down(PrentationMode)
            if(presentationCamera != null){

            
            if(inputManager.GetButtonDown("ChangeCamera") && presentationCamera != null){

                if(onPresentationCamera){
                    presentationCamera.enabled = false;
                    playerCamera.SetActive(true);
                    eventText.SetActive(true);
                    scope.SetActive(true);
                    playerToSpawn.transform.GetChild(2).GetChild(2).gameObject.SetActive(true);
                    playerToSpawn.GetComponent<SC_FPSController>().enabled = true;
                }

                else{
                    presentationCamera.enabled = true;
                    presentationCamera.enabled = true;
                    playerCamera.SetActive(false);
                    eventText.SetActive(false);
                    scope.SetActive(false);
                    playerToSpawn.transform.GetChild(2).GetChild(2).gameObject.SetActive(false);
                    playerToSpawn.GetComponent<SC_FPSController>().enabled = false;
                }

                onPresentationCamera = !onPresentationCamera;//Boolean cond modification always set to the opposite
            }
        }
        }

        if (!UnityEngine.Input.GetKeyDown(KeyCode.Escape)) 
        {
            escPul = false; // Detecta si no está pulsado
        }

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
                object[] content = new object[] {playerToSpawn.GetComponent<PhotonView>().Owner.NickName,animator.speed,"Stop&Replay"};
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others }; // You would have to set the Receivers to All in order to receive this event on the local client as well
                PhotonNetwork.RaiseEvent(2,content,raiseEventOptions,SendOptions.SendReliable);

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

                    GameObject[] playersInGame = GameObject.FindGameObjectsWithTag("Player");
                    foreach (GameObject player in playersInGame)
                    {
                        if (player.GetComponent<PhotonView>().Owner.NickName == (string) data[0])
                        {
                            switch ((string) data[2])
                            {
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
                    fileExplorer.GetComponent<FileExplorer>().downloadImages((string) data[0]);
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
                        eventObject.GetComponent<BackPresentation>().activate(false);
                    }
                    else
                    {
                        //Advance presentation
                        GameObject eventObject = GameObject.Find("Advance");
                        eventObject.GetComponent<AdvancePresentation>().activate(false);
                    }
                    break;
                }
            //Event FileExplorer Video
            case 24:
                {
                    loadingPressCanvas.enabled = true;
                    loadingPressCanvas.SetText("Loading");
                    object[] data = (object[]) photonEvent.CustomData;
                    fileExplorer.GetComponent<FileExplorer>().SetVideo((string) data[0],(string) data[1],compressor.Decompress((byte[]) data[2]));
                    break;
                }
            //Event FileExplorer Image
            case 25:
                {
                    loadingPressCanvas.enabled = true;
                    loadingPressCanvas.SetText("Loading");
                    object[] data = (object[]) photonEvent.CustomData;
                    fileExplorer.GetComponent<FileExplorer>().SetImage((byte[]) data[0]);
                    break;
                }

        }
    }

    //Function to set the Presentation Camera on Room Entered

   public void setPresentationCamera(Camera camera) {

        //When leaving presentation mode?
        if(camera == null)
        {
            //We deactivate the camera
            presentationCamera = null;

            //We deactivate UI
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

    
    
    //Cambio de mapa?
    public void ChangeRoom(string map)
    {
        if (map == "Mapa2")
        {
            //We reload the level
            PhotonNetwork.LoadLevel (map);
        }
    }
}
