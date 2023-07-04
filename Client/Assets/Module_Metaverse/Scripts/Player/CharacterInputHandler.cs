
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterInputHandler : MonoBehaviour
{
    //Movement
    Vector2 moveInputVector = Vector2.zero;
    Vector2 viewInputVector = Vector2.zero;


    //Input
    bool isJumpButtonPressed = false;

    //MiniMap markers
    List<int> playersInRoom;
    GameObject arrowMarkerPlayer;
    GetFriendMinimap getFriendMinimap;
    private GameObject miniMap;
    private GameObject miniMapCam;


    //Raycast
    [Header("Raycast")]
    //Raycast distance
    public float rayDistance = 3;

    //Raycast Object
    public GameObject raycastObject = null;

    //Raycast Timer
    private float targetTime = 0.5f;

    //Detect if Certain Object is being hit
    public bool HittingObject = false;


    //Camera
    [Header("Camera")]
    public float sensitivity;
    public float lookSpeed = 2.0f;
    public Camera playerCamera;
    LocalCameraHandler localCameraHandler;


    //Movement
    [Header("Movement")]
    public bool EnableMovement = true;
    public CharacterMovementHandler characterMovementHandler;
    

    //Presentation
    [Header("Presentation")]
    public Camera presentationCamera = null;
    public bool onPresentationCamera = false;

    public GameObject changeRoomPanel = null;

    //Drawing Plane ?
    private DrawLinesOnPlane drawingPlaneScript;


    //VoiceChat
    public VoiceManager voiceChat = new VoiceManager();//Manager for the voiceChat, not in scene object


    //Managers
    InputManager inputManager;
    PhotonManager photonManager;
    PauseManager pauseManager;
    UIManager uiManager;
    private bool OpenMiniMapPul;

    private void Awake()
    {
        inputManager = GameManager.FindInstance().GetComponent<InputManager>();
        photonManager = PhotonManager.FindInstance();

        pauseManager = PauseManager.FindInstance();
        uiManager = UIManager.FindInstance();

        getFriendMinimap = new GetFriendMinimap();
    }


    void Start()
    {
        //VoiceChat
        voiceChat.GetGameObjects();
        voiceChat.recorder.TransmitEnabled = false;

        //Activate Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Movement Handler -> Communicates input
        characterMovementHandler = this.gameObject.GetComponent<CharacterMovementHandler>();

        //ChatGPT
        //chatGPTActive = GameObject.FindObjectOfType<ChatGPTActive>();

        //Minimap
        miniMap = GameObject.Find("Canvasminimap");

        //Camera MiniMap
        miniMapCam = photonManager.CurrentPlayer.transform.GetChild(9).gameObject;
        miniMapCam.SetActive(true);

        //Activate localPlayer arrow on minimap
        arrowMarkerPlayer = photonManager.CurrentPlayer.transform.GetChild(7).gameObject;
        arrowMarkerPlayer.SetActive(true);

        //Activate friend market on minimap
        InitializeAsync();
    }

    public async void InitializeAsync()
    {
        List<Friend> listaAmigos = new List<Friend>();
        try
        {
            listaAmigos = await getFriendMinimap.GetFriendsConfirmedListAsync();
            Debug.Log("Lista Amigos: " + listaAmigos.Count);
            ActivateFriendsMarker(listaAmigos);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to get friends list: " + e.Message);
            // Handle the error case...
        }
    }

    public void ActivateFriendsMarker(List<Friend> listaAmigos)
    {
        // Encuentra el objeto Character(Clone)
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log("CharacterLenght: " + characters.Length);

        // Recorre cada objeto Character(Clone)
        foreach (GameObject character in characters)
        {
            string nickname = character.GetComponent<NetworkPlayer>().nickname.Value.ToString();

            foreach (Friend friend in listaAmigos)
            {

                // Friend was found
                if (nickname == friend.Username)
                {
                    Debug.Log(friend.Username + "Model(Clone) encontrado en " + character.name + " activating marker on minimap");

                    //Activate FriendMarker for this friend
                    character.transform.GetChild(8).transform.gameObject.SetActive(true);
                }

                else
                {
                    Debug.Log("Not a friend of yours");
                }
            }
        }

        Debug.Log(" All friendsMarkers activated on minimap!");
    }

    // Update is called once per frame
    void Update()
    {
        //This script should only get the inputs (W,A,S,D, Mouse && other keys)

        //Just neeeded in camera
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 1.0f);

        //OK
        if (inputManager.GetButtonDown("MuteVoice") && !pauseManager.IsPaused)
            voiceChat.MuteAudio(photonManager.UserStatus);


        if (localCameraHandler == null)
        {

            localCameraHandler = GetComponentInChildren<LocalCameraHandler>();
            playerCamera = localCameraHandler.gameObject.GetComponent<Camera>();
        }

        // Raycast Logic
        Raycast();

        //Emote Wheel Logic
        CheckWheel();

        //Pause Menu Logic
        CheckPause();

        //Presentation Camera Logic

        CheckPresentationCamera();

        //Minimap Logic
        CheckMiniMap();

        //MovementLogic
        InputMovement();


    }

    //OK
    /// <summary>
    /// Share Movement Data with other players
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
    /// Change the state of camera when you are in the presentation zone
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
            uiManager.HidePresentationText();
        }

        else
        {
            //We obtain the camera
            presentationCamera = camera;

            //We activate UI
            uiManager.ShowPresentationText();
        }
    }

    /// <summary>
    /// Changes the EmoteWheel State
    /// </summary>
    public void CheckWheel()
    {

        //InGame
        if (!pauseManager.IsPaused)
        {
            if (inputManager.GetButtonDown("Wheel"))
            {

                pauseManager.Pause();
                uiManager.OpenEmoteWheel();
                EnableMovement = false;

            }
        }
        //Pause
        else
        {

            if (inputManager.GetButtonDown("Wheel") && uiManager.EmoteWheel.activeSelf)
            {
                pauseManager.Unpause();
                uiManager.CloseEmoteWheel();
                EnableMovement = true;
            }
        }
    }

    /// <summary>
    /// Change the Pause State
    /// </summary>
    public void CheckPause()
    {
        //InGame
        if (!pauseManager.IsPaused)
        {
            //ESC key down(PauseMenu)
            if ((Input.GetKeyDown(KeyCode.Escape)))
            {
                //setPause();
                pauseManager.Pause();
                uiManager.OpenPauseMenu();
                EnableMovement = false;

            }
        }
        //Pause
        else
        {

            if (Input.GetKeyDown(KeyCode.Escape))
            {

                pauseManager.Unpause();
                uiManager.CloseNonPlayerUI();
                EnableMovement = true;

                if (changeRoomPanel != null)
                    changeRoomPanel.SetActive(false);

                //Make method
                if (onPresentationCamera)
                {
                    ChangeCamera();
                }
            }
        }
    }

    /// <summary>
    /// Change the current Camera being used by the player
    /// </summary>
    private void ChangeCamera()
    {
        //Exit Presentation Camera
        if (onPresentationCamera)
        {
            presentationCamera.enabled = false;
            playerCamera.enabled = true;
            uiManager.ShowPresentationText();
            uiManager.SetUIOn();
            pauseManager.Unpause();

            EnableMovement = true;

            if (SceneManager.GetActiveScene().name == "LobbyOficial")
            {
                Debug.Log("Deactivate Drawline");
                drawingPlaneScript = GameObject.Find("Renderer").GetComponent<DrawLinesOnPlane>();
                drawingPlaneScript.enabled = false;
            }
        }

        //Enter Presentation Camera
        else
        {
            presentationCamera.enabled = true;
            playerCamera.enabled = false;
            uiManager.SetUIOff();
            pauseManager.Pause();
            EnableMovement = false;


            if (SceneManager.GetActiveScene().name == "LobbyOficial")
            {
                Debug.Log("Activate Drawline");
                if (drawingPlaneScript == null) drawingPlaneScript = GameObject.Find("Renderer").GetComponent<DrawLinesOnPlane>();
                drawingPlaneScript.enabled = true;
            }
        }

        onPresentationCamera = !onPresentationCamera;//Boolean cond modification always set to the opposite
    }

    /// <summary>
    /// Confirms if presentationCamera exists
    /// </summary>
    public void CheckPresentationCamera()
    {
        //K key down(PresentationMode)
        if (presentationCamera != null)
        {
            if (inputManager.GetButtonDown("ChangeCamera") && presentationCamera != null)
            {

                ChangeCamera();
            }
        }
    }

    /// <summary>
    /// Changes the MiniMap State
    /// </summary>
    public void CheckMiniMap()
    {
        //InGame
        if (!pauseManager.IsPaused)
        {
            //OK but minimap Logic in another script
            if ((inputManager.GetButtonDown("OpenMiniMap") && !OpenMiniMapPul))
            {
                miniMap.transform.GetChild(0).gameObject.SetActive(true); //MinimapMask
                miniMap.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true); //minimapRender
                miniMap.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true); //minimapBorder
                OpenMiniMapPul = true;
            }
        }

        else if ((inputManager.GetButtonDown("OpenMiniMap")) && !OpenMiniMapPul)
        {
            miniMap.transform.GetChild(0).gameObject.SetActive(false); //MinimapMask
            miniMap.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false); //minimapRender
            miniMap.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false); //minimapBorder
        }
    }

    /// <summary>
    /// Manages the movement of the player
    /// </summary>
    public void InputMovement()
    {
        //Movement is active
        if (EnableMovement)
        {
            if (!localCameraHandler.enabled) localCameraHandler.enabled = true;
            if (!characterMovementHandler.enabled) characterMovementHandler.enabled = true;
            //View input
            viewInputVector.x = Input.GetAxis("Mouse X") / sensitivity;
            viewInputVector.y = Input.GetAxis("Mouse Y") * -1 / sensitivity; //Invert the mouse look

            if (Input.GetButton("Jump"))
                isJumpButtonPressed = true;

            //Add values to camera
            if (localCameraHandler != null) localCameraHandler.SetViewInputVector(viewInputVector);

        }
        //Movement is not active
        else
        {
            if (localCameraHandler.enabled) localCameraHandler.enabled = false;
            if (characterMovementHandler.enabled) characterMovementHandler.enabled = false;
        }
    }

    /// <summary>
    /// Manages the raycast interactions
    /// </summary>
    public void Raycast()
    {
        //UI script
        if (HittingObject && !pauseManager.IsPaused && onPresentationCamera == false)
            uiManager.ShowEventText();

        //Raycast script
        if (!pauseManager.IsPaused)
        {
            targetTime -= Time.deltaTime;
            //Raycast
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, rayDistance, LayerMask.GetMask("Interactive")))
            {
                if (raycastObject == null)
                {
                    raycastObject = hit.transform.gameObject;

                    uiManager.ShowEventText();
                    HittingObject = true;
                }
                //RaycastObject
                else if (raycastObject != hit.transform.gameObject)
                {
                    raycastObject = hit.transform.gameObject;

                    uiManager.ShowEventText();
                    HittingObject = true;
                }

                //If the user interacts, activate the event
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
                    raycastObject = null;
                    uiManager.HideEventText();
                    HittingObject = false;
                }
            }
        }
    }
}
