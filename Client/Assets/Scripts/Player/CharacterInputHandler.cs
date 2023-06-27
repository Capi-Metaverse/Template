
using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterInputHandler : MonoBehaviour
{
    Vector2 moveInputVector = Vector2.zero;
    Vector2 viewInputVector = Vector2.zero;
    bool isJumpButtonPressed = false;

    public float sensitivity;
    public float lookSpeed = 2.0f;

    //Raycast

    //Raycast distance
    public float rayDistance = 3;
    //Raycast active
    public bool active = false;
    public float targetTime = 0.5f;
    public GameObject raycastObject = null;
    //Detect if Certain Object is being hit
    bool HittingObject = false;
    public Camera playerCamera;

    LocalCameraHandler localCameraHandler;
    public bool escPul;//Reference if ESC key is pushed or not(ESC opens the Menu and youï¿½ll be on Pause State)
    public GameObject Pause;//Pause is an object in scene map, you can see it as the manager of the pause state
    public GameObject Settings;//The same as Pause but for settings, the state will be Pause too cause the setting are accesible from Pause
    public GameObject UICard;
    public GameObject UICardOtherUser;
    private GameObject miniMap;


    //PlayerUIPrefab
    GameObject scope;
    GameObject micro;//Actually this is the microphone in game
    public GameObject eventText;
    public GameObject eventTextK;
    public GameObject changeRoomPanel = null;

    ChatGPTActive chatGPTActive;

    public GameObject emoteWheel;

    //Drawing Plane
    private DrawLinesOnPlane drawingPlaneScript;


    public VoiceManager voiceChat = new VoiceManager();//Manager for the voiceChat, not in scene object
    CharacterMovementHandler characterMovementHandler;

    GameManager gameManager;
    InputManager inputManager;
    PhotonManager photonManager;

    //Presentation
    public Camera presentationCamera = null;
    public bool onPresentationCamera = false;


    public string nickname;
    private bool OpenMiniMapPul;

    private void Awake()
    {
        gameManager = GameManager.FindInstance().GetComponent<GameManager>();
        inputManager = GameManager.FindInstance().GetComponent<InputManager>();
        photonManager = PhotonManager.FindInstance();
    }


    void Start()
    {
        voiceChat.GetGameObjects();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        voiceChat.recorder.TransmitEnabled = false;
        GameObject currentPlayer = PhotonManager.FindInstance().CurrentPlayer;
        characterMovementHandler = this.gameObject.GetComponent<CharacterMovementHandler>();
        Settings = GameObject.Find("Menus").transform.GetChild(0).gameObject;
        Pause = GameObject.Find("Menus").transform.GetChild(1).gameObject;
        emoteWheel = currentPlayer.transform.GetChild(6).GetChild(0).gameObject;
        Debug.Log(emoteWheel);
        //PlayerUIPrefab
        micro = currentPlayer.transform.GetChild(3).GetChild(0).gameObject;//Micro
        scope = currentPlayer.transform.GetChild(3).GetChild(1).gameObject;//Scope
        eventText = currentPlayer.transform.GetChild(3).GetChild(2).gameObject;
        eventTextK = currentPlayer.transform.GetChild(3).GetChild(3).gameObject;

        //ChatGPT
        chatGPTActive = GameObject.FindObjectOfType<ChatGPTActive>();
        UICard = GameObject.Find("Menus").transform.GetChild(4).gameObject;
        UICardOtherUser = GameObject.Find("Menus").transform.GetChild(5).gameObject;

        //Minimap
        miniMap = GameObject.Find("Canvasminimap");

        //seteamos el estado para que este InGame, esto hay que cambiarlo
        photonManager.UserStatus = UserStatus.InGame;
    }

    // Update is called once per frame
    void Update()
    {
        
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 1.0f);

        if (inputManager.GetButtonDown("MuteVoice") && photonManager.UserStatus == UserStatus.InGame)
            voiceChat.MuteAudio(photonManager.UserStatus);
        nickname = this.gameObject.GetComponent<NetworkPlayer>().nickname.ToString();

       
        if (localCameraHandler == null) {
            
            localCameraHandler = GetComponentInChildren<LocalCameraHandler>();
            playerCamera = localCameraHandler.gameObject.GetComponent<Camera>();
        }
     
        if (HittingObject && photonManager.UserStatus != UserStatus.InPause && onPresentationCamera==false)
            eventText.SetActive(true);

        if (photonManager.UserStatus != UserStatus.InPause)
        {
            targetTime -= Time.deltaTime;
            //Raycast
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, rayDistance, LayerMask.GetMask("Interactive")))
            {
                if (raycastObject == null)
                {
                    raycastObject = hit.transform.gameObject;

                    eventText.SetActive(true);
                    HittingObject = true;
                }
                //RaycastObject
                else if (raycastObject != hit.transform.gameObject)
                {
                    raycastObject = hit.transform.gameObject;

                    eventText.SetActive(true);
                    HittingObject = true;
                }

                //If the user interacts, activate the event
                //if (inputManager.GetButtonDown("Interact") && targetTime <= 0)
                if (inputManager.GetButtonDown("Interact") && targetTime <= 0)
                {
                    //Cooldown timer
                    targetTime = 0.5f;

                    //Retrieve Parent Object and call event
                    GameObject eventObject = hit.transform.gameObject;
                    Debug.Log(eventObject);
                    eventObject.GetComponent<IMetaEvent>().eventObject = eventObject;
                    eventObject.GetComponent<IMetaEvent>().activate(true);
                }
            }

            else
            {

                if (raycastObject != null)
                {
                    //raycastObject.GetComponent<Outline>().enabled = false;
                    raycastObject = null;
                    eventText.SetActive(false);
                    HittingObject = false;
                }
            }
        }

            //Pause
            if (!Input.GetKeyDown(KeyCode.Escape))
            {

                escPul = false; // Detecta si no esta pulsado

            }
            if (!inputManager.GetButtonDown("OpenMiniMap"))
            {
                OpenMiniMapPul = false; // Detecta si no esta pulsado
            }

        switch (photonManager.UserStatus)
            {
                case UserStatus.InGame:
                    {

                    if ((inputManager.GetButtonDown("OpenMiniMap") && !OpenMiniMapPul))
                    {
                        miniMap.transform.GetChild(0).gameObject.SetActive(true);
                        OpenMiniMapPul = true;
                    }

                    //ESC key down(PauseMenu)
                    if ((Input.GetKeyDown(KeyCode.Escape) && !escPul))
                        {
                            setPause();
                        }

                        if (inputManager.GetButtonDown("Wheel") && !escPul)
                        {
                            setEmoteWheel();
                        }

                        //K key down(PresentationMode)
                        if (presentationCamera != null)
                        {
                            if (inputManager.GetButtonDown("ChangeCamera") && presentationCamera != null)
                            {

                                if (onPresentationCamera)
                                {
                                    presentationCamera.enabled = false;
                                    playerCamera.enabled = true;
                                    eventTextK.SetActive(true);
                                    ActiveALL();

                                    if (SceneManager.GetActiveScene().name == "LobbyOficial")
                                    {
                                        Debug.Log("Deactivate Drawline");
                                        drawingPlaneScript = GameObject.Find("Plane").GetComponent<DrawLinesOnPlane>();
                                        drawingPlaneScript.enabled = false;
                                    }
                                }

                                else
                                {
                                    presentationCamera.enabled = true;
                                    playerCamera.enabled = false;
                                    eventText.SetActive(false);
                                    DeactivateALL();
                                photonManager.UserStatus = UserStatus.InGame;

                                    if (SceneManager.GetActiveScene().name == "LobbyOficial")
                                    {
                                        Debug.Log("Activate Drawline");
                                        if (drawingPlaneScript == null) drawingPlaneScript = GameObject.Find("Plane").GetComponent<DrawLinesOnPlane>();
                                        drawingPlaneScript.enabled = true;
                                    }
                            }

                                onPresentationCamera = !onPresentationCamera;//Boolean cond modification always set to the opposite
                            }
                        }
                        break;
                    }
            case UserStatus.InPause:
                {
                    if (Input.GetKeyDown(KeyCode.Escape) && !escPul)
                    {
                        setJuego();

                        if (changeRoomPanel != null)
                            changeRoomPanel.SetActive(false);
                    }

                    if ((inputManager.GetButtonDown("OpenMiniMap")) && !OpenMiniMapPul)
                    {
                        miniMap.transform.GetChild(0).gameObject.SetActive(false);

                    }
                    break;
                }

            default:
                    Debug.Log(photonManager.UserStatus);
                    break;
            }


            //View input
            viewInputVector.x = Input.GetAxis("Mouse X") / sensitivity;
            viewInputVector.y = Input.GetAxis("Mouse Y") * -1 / sensitivity; //Invert the mouse look

            //Move Input
            moveInputVector.x = Input.GetAxis("Horizontal");
            moveInputVector.y = Input.GetAxis("Vertical");

            if (Input.GetButton("Jump"))
                isJumpButtonPressed = true;

            if (localCameraHandler != null) localCameraHandler.SetViewInputVector(viewInputVector);
        }
    
    /// <summary>
    /// To all the people can see the movement
    /// </summary>
    /// <returns></returns>
    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();

        //Aim data
        networkInputData.aimForwardVector = localCameraHandler.transform.forward;

        //Move data
        networkInputData.movementInput = moveInputVector;

        //Jump data
        networkInputData.isJumpPressed = isJumpButtonPressed;

        isJumpButtonPressed = false;

        return networkInputData;

    }

    /// <summary>
    /// Change the state of camera when you are in a zone of presentation
    /// </summary>
    /// <param name="camera"></param>
    public void setPresentationCamera(Camera camera)
    {

        //When leaving presentation mode?
        if (camera == null)
        {
            //We deactivate the camera
            presentationCamera = null;

            //We deactivate UI
            eventTextK.SetActive(false);
        }

        else
        {
            //We obtain the camera
            presentationCamera = camera;

            //We activate UI
            eventTextK.SetActive(true);
        }
    }

    /// <summary>
    /// DeactivateALL
    /// </summary>
    public void DeactivateALL()
    {
        escPul = true;
        photonManager.UserStatus = UserStatus.InPause;
        
    //playerToSpawn.GetComponent<SC_FPSController>().enabled = false;


    
        //AQUI IRA EL FIND DEL CHARACTER CONTROL PARA DESACTIVAR
        //AQUI IRA EL FIND DEL PLAYERCAMERA PARA DESACTIVARLA
        characterMovementHandler.enabled=false;
        localCameraHandler.enabled=false;


        //Deactivate presentation text
        eventTextK.SetActive(false);
        eventText.SetActive(false);

        micro.SetActive(false);
        scope.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // Desactiva el bloqueo cursor
    }

    /// <summary>
    /// ActiveALL
    /// </summary>
    public void ActiveALL()
    {

        //playerToSpawn.GetComponent<SC_FPSController>().enabled = true;



        // AQUI IRA EL FIND DEL CHARACTER CONTROL PARA ACTIVAR
        //AQUI IRA EL FIND DEL PLAYERCAMERA PARA ACTIVAR
        characterMovementHandler.enabled = true;
        localCameraHandler.enabled = true;
        photonManager.UserStatus = UserStatus.InGame;


        //Deactivate presentation text
        if (presentationCamera!=null)
            eventTextK.SetActive(true);


        Cursor.visible = false;
        scope.SetActive(true);
        micro.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked; // Desactiva el bloqueo cursor 
    }

    public void setPause()
    {
       

        //Pause canvas
        
        Pause.SetActive(true);
       
        DeactivateALL();
        //Escape activado

    }


    public void setEmoteWheel()
    {
        //Pause canvas

        emoteWheel.SetActive(true);

        DeactivateALL();
        //Escape activado

    }


    public void setJuego()
    {

        //Activate Settings Window and stop
        chatGPTActive.activate(false);
        Settings.SetActive(false);
        Pause.SetActive(false);
        emoteWheel.SetActive(false);
        UICardOtherUser.SetActive(false);
        Cursor.visible = false;
        UICard.SetActive(false);
        //States and Reactivate all


        ActiveALL();
        Debug.Log(photonManager.UserStatus);

    }

 
}
