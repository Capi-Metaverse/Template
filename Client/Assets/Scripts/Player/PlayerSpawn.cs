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
public class PlayerSpawn : MonoBehaviourPunCallbacks
{
    InputManager inputManager;
    ChatGPTActive chatGPTActive;
    
    /*------------MANAGERS AS GAMEOBJECTS IN SCENE MAP----------------*/
    private AudioController voiceChat;//Manager for the voiceChat, not in scene object
    public Animator animator;//Animation manager
    public GameObject Pausa;//Pausa is an object in scene map, you can see it as the manager of the pause state
    public GameObject chatManager;//Also in scene map is the manager for the TextChat(T to open TextChat)
    public GameObject Settings;//The same as Pausa but for settings, the state will be Pausa too cause the setting are accesible from Pausa

    /*--------------------BOOLEANS FOR CONDITIONAL CHECKING---------------------*/
    public Estados estado; //With this we keep track of the current state so we can use it in conditionals. States are (Game, Pause)
    public bool onPresentationCamera = false;//Boolean to know if pressMode is on or not
    public bool escPul;//Reference if ESC key is pushed or not(ESC opens the Menu and you´ll be on Pause State)
    bool TPul;//Reference if T key is pushed or not(T opens the TextChat)

    /*-------------UTILITY VARIABLES-----------------*/
    public GameObject[] playerPrefabs;//This are the player models(ReadyPlayerMe), all of them in a list
    public Camera presentationCamera = null;//Outside of the PlayerPrefab. With K you can change from playerCam to this cam that look ortographically to the presentation
    public GameObject playerCamera;//Inside of the PlayerPrefab
    public GameObject playerToSpawn;//Player Character
    public Transform[] spawnPoints;//Array with points corresponding to points in the map, the players spawn randomly
    static Vector3 spawnPoint;//A 3 coordinate point where the player must spawn
    public GameObject scope;//Actually this is the crosshair in game
    public GameObject micro;//Actually this is the microphone in game
    public GameObject eventTextK;//A text for displaying when an event happens
    GameObject canvasGPT;
    
    public enum Estados
    {
        Juego,Pausa
    }
    
    /*-------------METHODS-----------------*/
     private void Start()
    {
        
        inputManager = GameObject.FindObjectOfType<InputManager>();
        chatGPTActive = GameObject.FindObjectOfType<ChatGPTActive>();
        //We activate the message Queue again
        PhotonNetwork.IsMessageQueueRunning = true;
        GameObject[] playersInGame = GameObject.FindGameObjectsWithTag("Player");

        canvasGPT = GameObject.Find("Enviroment").transform.Find("CanvasChatGPT").gameObject;//Getting canvas Object

        foreach (GameObject player in playersInGame)
        {
            if (player.gameObject.GetComponent<PhotonView>().IsMine) 
            {
                canvasGPT.GetComponent<Canvas>().worldCamera = player.transform.GetChild(1).GetComponent<Camera>();
            }  
        }
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

        if (!UnityEngine.Input.GetKeyDown(KeyCode.Escape)) 
        {
            escPul = false; // Detecta si no está pulsado
        }

        switch (estado)
        {
            case Estados.Juego:
            {
            //ESC key down(PauseMenu)
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && !escPul)
            {
               setPausa();
               
            }

            //T key down(TextChat)
            if (UnityEngine.Input.GetKeyDown(KeyCode.T) && !TPul)
            {
                TPul = true; //Escape is activated, true
                //Start Animation
                animator.speed = 0;
                object[] content =new object[] {playerToSpawn.GetComponent<PhotonView>().Owner.NickName,animator.speed,"Stop&Replay"};
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others }; // You would have to set the Receivers to All in order to receive this event on the local client as well
                PhotonNetwork.RaiseEvent(2,content,raiseEventOptions,SendOptions.SendReliable);

                UnityEngine.Debug.Log("T pulsada");
                chatManager.SetActive(true);
                chatManager.GetComponent<PhotonChatManager>().ChatConnectOnClick();
                 //State and Cursor
                estado = Estados.Pausa;
                Cursor.visible = true;
                DesactiveALL();
               
                UnityEngine.Debug.Log (estado);
            }

            //K key down(PrentationMode)
            if(presentationCamera != null)
            {
                if(inputManager.GetButtonDown("ChangeCamera") && presentationCamera != null)
                {

                    if(onPresentationCamera){
                        presentationCamera.enabled = false;
                        playerCamera.SetActive(true);
                        ActiveALL();
                    }

                    else{
                        
                        presentationCamera.enabled = true;
                        playerCamera.SetActive(false);
                        playerToSpawn.GetComponent<SC_FPSController>().eventText.SetActive(false);
                        DesactiveALL();
                       
                    }

                    onPresentationCamera = !onPresentationCamera;//Boolean cond modification always set to the opposite
                }
            }
            break;
        }  
        case Estados.Pausa:
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && !escPul)
            {
               setJuego();
            }
            break;
        }

            default:
                break;
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
            eventTextK = GameObject.Find("PlayerUIPrefab").transform.GetChild(3).gameObject;
            eventTextK.SetActive(false);
            eventTextK = null;
        }

        else{

            //We obtain the camera
            presentationCamera = camera;

            //We activate UI
            eventTextK = GameObject.Find("PlayerUIPrefab").transform.GetChild(3).gameObject;
            eventTextK.SetActive(true);
        }   
    }

    //DesactiveAll
    public void DesactiveALL()
    {
        
        playerToSpawn.GetComponent<SC_FPSController>().enabled = false;

         //Deactivate presentation text
        if(eventTextK != null) 
        {
            eventTextK.SetActive(false);
        }
    
    ///DESACTIVAR LAS LETRAS DE PULSA E

        
        scope.SetActive(false);
        micro.SetActive(false);
        Cursor.lockState = CursorLockMode.None; // Desactiva el bloqueo cursor
    }

    //ActiveALL
    public void ActiveALL()
    {
        
        playerToSpawn.GetComponent<SC_FPSController>().enabled = true;

          //Deactivate presentation text
        if(eventTextK != null) 
        {
            eventTextK.SetActive(true);
        }
    
    ///DESACTIVAR LAS LETRAS DE PULSA E

        Cursor.visible = false;
        scope.SetActive(true);
        micro.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked; // Desactiva el bloqueo cursor 
    }

    public void instantiatePlayer()
    {
        //We are connected now inside of a room, so we instantiate everything
        //States
        estado = Estados.Juego;
        escPul = false;
        TPul = false;
        chatManager.SetActive(true);

        //We check if it's the first time the user entered the room.
        int randomNumber = Random.Range(0, spawnPoints.Length);
        spawnPoint = spawnPoints[randomNumber].position;
        
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
        micro = GameObject.Find("PlayerUIPrefab").transform.GetChild(0).gameObject;//Micro


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

    }

    public void setState(int number){

        switch(number){
            case 0: 
                estado = Estados.Juego;
                break;
            case 1: 
                estado = Estados.Pausa;
                break;
            default:
                break;
        }

        

    }

    public void setPausa(){

         escPul = true;
                playerToSpawn.GetComponent<SC_FPSController>().eventText.SetActive(false);
                
                //Start Animator
                animator.speed = 0;
                object[] content =new object[] {playerToSpawn.GetComponent<PhotonView>().Owner.NickName,animator.speed,"Stop&Replay"};
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others }; // You would have to set the Receivers to All in order to receive this event on the local client as well
                PhotonNetwork.RaiseEvent(2,content,raiseEventOptions,SendOptions.SendReliable);

                //Pause canvas
                
                Pausa.SetActive(true);
                //RoomName on Settings
                GameObject SalaText = Pausa.transform.Find("RoomName").gameObject;
                Scene scene = SceneManager.GetActiveScene();
                SalaText.GetComponent<TMP_Text>().text = ((string) PhotonNetwork.CurrentRoom.CustomProperties["Name"]) + " " + scene.name;
                //State and Cursor
                estado = Estados.Pausa;
                Cursor.visible = true;
                DesactiveALL();
                 //Escape activado
                UnityEngine.Debug.Log (estado);

    }

    public void setJuego(){

         //Activate scope
                chatGPTActive.activate(false);
                //Stop Animation
                animator.speed = 1;
                object[] content = new object[] {playerToSpawn.GetComponent<PhotonView>().Owner.NickName,animator.speed,"Stop&Replay"};
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others }; // You would have to set the Receivers to All in order to receive this event on the local client as well
                PhotonNetwork.RaiseEvent(2,content,raiseEventOptions,SendOptions.SendReliable);

                //Activate Settings Window and stop
                
                Settings.SetActive(false);
                Pausa.SetActive(false);
                chatManager.SetActive(false);
                Cursor.visible = false;
                TPul = false;
                //States and Reactivate all
                estado = Estados.Juego;
                ActiveALL();
                UnityEngine.Debug.Log (estado);
        
    }
}
