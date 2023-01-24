using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using TMPro;

using Newtonsoft.Json.Linq;

using System.IO;

public class PlayerSpawn : MonoBehaviourPunCallbacks, IOnEventCallback
{
    private Estados estado;
    private TestHome voiceChat;
    public Animator animator;
    public GameObject[] playerPrefabs;
    public Transform[] spawnPoints;
    public GameObject Pausa;
    public GameObject chatManager;
    public GameObject Settings;
    public GameObject fileExplorer;

    //Map Variables
    public string mapName;
    bool escPul;
    bool TPul;
    bool LPul;
    //Static variables
    static bool reload = false;
    static Vector3 spawnPoint;

    //Player Character
    GameObject playerToSpawn;

    private void Start() {

        //States
        estado = Estados.Juego;
        escPul=false;
        TPul=false;

        chatManager.SetActive(true);

        //We check if it's the first time the user entered the room.
        if(reload == false){
        int randomNumber = Random.Range(0, spawnPoints.Length);
        spawnPoint = spawnPoints[randomNumber].position;
        }
        voiceChat=GameObject.Find("VoiceManager").GetComponent<TestHome>();
        
        //Random avatar character
        if(PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == null || (int) PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == 8)
        {
           
            int value = Random.Range(0,5);
            PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] = value;    
           
            UnityEngine.Debug.Log(PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] = value);
        }
        
        //Player instantation
        playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
       //CS script for movement activated
        playerToSpawn = (GameObject) PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint, Quaternion.identity);
        
        //-------------------------ACTIVATING CAM AND MOVEMENT ONLY ON LOCAL PLAYER------------------------//
        //this is because we only want the camera and the movement activated for the local player so by default the prefab have both cam and script deactivated. Here is where we activate it right in time, when everithing´s prepared.
        playerToSpawn.GetComponent<SC_FPSController>().enabled = true;
        playerToSpawn.transform.Find("PlayerCamera").gameObject.SetActive(true);//Camera of the player
        
        //-------------------------ACTIVATING UI------------------------//
        //Prefab of the UI for VoiceChat
        playerToSpawn.transform.Find("PlayerUIPrefab").gameObject.SetActive(true);

        //UI for the Text Chat
        /*
        
        */
        //-------------------------ACTIVATING UI END------------------------//

        animator = playerToSpawn.transform.GetChild(0).GetComponent<Animator>();
        voiceChat.CheckMicroImage();
        PhotonNetwork.IsMessageQueueRunning = true;
}

public override void OnConnectedToMaster()
{
    //Not needed??
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
}


private void Update() {

   /* 
    if (estado == Estados.Juego)
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.L) && !LPul)
        {
            LPul=true;
            playerToSpawn.transform.Find("Canvas").gameObject.SetActive(true);
            estado = Estados.Pausa;
        }
        else if (!UnityEngine.Input.GetKeyDown(KeyCode.L))
        {
            LPul=false;
        }
    }*/
    
    //Pause State
    if (estado == Estados.Juego)
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && !escPul)
            {
                animator.speed=0;
                Pausa.SetActive(true);
                //Time.timeScale = 0;
                playerToSpawn.GetComponent<SC_FPSController>().enabled = false;

                Cursor.visible = true;   
                Cursor.lockState = CursorLockMode.None; // Desactiva el bloqueo cursor
                estado = Estados.Pausa;
                escPul=true; //Escape activado
                UnityEngine.Debug.Log(estado);
            }
    }      
    
    if (!UnityEngine.Input.GetKeyDown(KeyCode.Escape)) escPul=false; // Detecta si no está pulsado

    //Game State
    if (estado == Estados.Pausa)
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && !escPul)
        {
            animator.speed=1;
            Settings.SetActive(false);
            Pausa.SetActive(false);
            chatManager.SetActive(false);
            TPul=false;
            Time.timeScale = 1;
            playerToSpawn.GetComponent<SC_FPSController>().enabled = true;
            Cursor.visible = false;   
            Cursor.lockState = CursorLockMode.Locked; // Menu de opciones, para que se bloquee la camara 
            estado = Estados.Juego;
            UnityEngine.Debug.Log(estado);  
        }
    }

     if (estado == Estados.Juego)
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.T) && !TPul)
            {
                animator.speed=0;
                UnityEngine.Debug.Log("T pulsada");
                chatManager.SetActive(true);
                chatManager.GetComponent<PhotonChatManager>().ChatConnectOnClick();
                playerToSpawn.GetComponent<SC_FPSController>().enabled = false;

                Cursor.visible = true;   
                Cursor.lockState = CursorLockMode.None; // Desactiva el bloqueo cursor
                estado = Estados.Pausa;
                TPul=true; //Escape activado
                UnityEngine.Debug.Log(estado);
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
   if(photonEvent.Code == 1)
   {
    PhotonNetwork.IsMessageQueueRunning = false;
    //We maintain the same state between reloads.
    reload = true;
    spawnPoint = playerToSpawn.transform.position;
   
   //We reload the level
   PhotonNetwork.LoadLevel(mapName);
   }

    if(photonEvent.Code == 21)
   {
    object[] data = (object[])photonEvent.CustomData;
    
   GameObject eventObject = GameObject.Find((string) data[0]);
   
   eventObject.GetComponent<Lamp>().activate();

   }

   if(photonEvent.Code == 22)
   {
    object[] data = (object[])photonEvent.CustomData;
    fileExplorer.SetActive(true);
    fileExplorer.GetComponent<FileExplorer>().downloadImages((JObject) data[0]);
    
    

   }
}

//Recibir eventos
public void ChangeRoom(string map)
{
   if(map == "Mapa2")
   {
   //We reload the level
   PhotonNetwork.LoadLevel(map);
   }
}
}
