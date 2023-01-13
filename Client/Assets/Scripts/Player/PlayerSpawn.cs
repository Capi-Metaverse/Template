using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerSpawn : MonoBehaviourPunCallbacks, IOnEventCallback
{
    private Estados estado;
    private TestHome voiceChat;
    public GameObject[] playerPrefabs;
    public Transform[] spawnPoints;
    public GameObject Pausa;
    public GameObject chatManager;
    public GameObject Settings;
  

    //Map Variables
    public string mapName;
    bool escPul;
    bool TPul;
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
        if(PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == null || (int) PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == 6)
        {
           
            int value = Random.Range(0,5);
            PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] = value;    
           
            Debug.Log(PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] = value);
            
            
        }
       
            //Player and camera instantation
            playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
            playerToSpawn = (GameObject) PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint, Quaternion.identity);
           
            playerToSpawn.GetComponent<SC_FPSController>().enabled = true;
            playerToSpawn.transform.Find("PlayerCamera").gameObject.SetActive(true);
            playerToSpawn.transform.Find("PlayerUIPrefab").gameObject.SetActive(true);
            voiceChat.CheckMicroImage();


            PhotonNetwork.IsMessageQueueRunning = true;


}

public override void OnConnectedToMaster()
{
    //Not needed??
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
}

private void Update() {

    //Pause State
    if (estado == Estados.Juego)
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !escPul)
            {
                Pausa.SetActive(true);
                //Time.timeScale = 0;
                playerToSpawn.GetComponent<SC_FPSController>().enabled = false;

                Cursor.visible = true;   
                Cursor.lockState = CursorLockMode.None; // Desactiva el bloqueo cursor
                estado = Estados.Pausa;
                escPul=true; //Escape activado
                Debug.Log(estado);
            }
    }      
    
    if (!Input.GetKeyDown(KeyCode.Escape)) escPul=false; // Detecta si no está pulsado

    //Game State
    if (estado == Estados.Pausa)
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !escPul)
        {
            Settings.SetActive(false);
            Pausa.SetActive(false);
            chatManager.SetActive(false);
            Time.timeScale = 1;
            playerToSpawn.GetComponent<SC_FPSController>().enabled = true;
            Cursor.visible = false;   
            Cursor.lockState = CursorLockMode.Locked; // Menu de opciones, para que se bloquee la camara 
            estado = Estados.Juego;
            Debug.Log(estado);  
        }
    }

     if (estado == Estados.Juego)
    {
        if (Input.GetKeyDown(KeyCode.T) && !TPul)
            {
                Debug.Log("T pulsada");
                chatManager.SetActive(true);
                chatManager.GetComponent<PhotonChatManager>().ChatConnectOnClick();
                playerToSpawn.GetComponent<SC_FPSController>().enabled = false;

                Cursor.visible = true;   
                Cursor.lockState = CursorLockMode.None; // Desactiva el bloqueo cursor
                estado = Estados.Pausa;
                TPul=true; //Escape activado
                Debug.Log(estado);
            }
    }      
    
    if (!Input.GetKeyDown(KeyCode.T)) TPul=false; // Detecta si no está pulsado

    //Estado juego
    if (estado == Estados.Pausa)
    {
        if (Input.GetKeyDown(KeyCode.T) && !TPul)
        {
           
            chatManager.SetActive(false);
            Time.timeScale = 1;
            playerToSpawn.GetComponent<SC_FPSController>().enabled = true;
            Cursor.visible = false;   
            Cursor.lockState = CursorLockMode.Locked; // Menu de opciones, para que se bloquee la camara 
            estado = Estados.Juego;
            Debug.Log(estado);  
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
         
    GameObject child = GameObject.Find("/Lamp/Bulb");
   
    child.GetComponent<Lamp>().activate();


 
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
