
using System;
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



    //Raycast
    [Header("Raycast")]
    //Raycast distance
    public float rayDistance = 3;

    //Raycast Object
    public GameObject raycastObject = null;

    //Raycast active
    private bool active = false;

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
    
    private GameObject miniMap;

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



    

    private void Awake()
    {
        inputManager = GameManager.FindInstance().GetComponent<InputManager>();
        photonManager = PhotonManager.FindInstance();

        pauseManager = PauseManager.FindInstance();
        uiManager = UIManager.FindInstance();

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

        //Minimap Not needed
        miniMap = GameObject.Find("Canvasminimap").transform.GetChild(0).gameObject;
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
                drawingPlaneScript = GameObject.Find("Plane").GetComponent<DrawLinesOnPlane>();
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
                if (drawingPlaneScript == null) drawingPlaneScript = GameObject.Find("Plane").GetComponent<DrawLinesOnPlane>();
                drawingPlaneScript.enabled = true;
            }
        }

        onPresentationCamera = !onPresentationCamera;//Boolean cond modification always set to the opposite

    }

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

    public void CheckMiniMap()
    {
        //InGame
        if (!pauseManager.IsPaused)
        {
            //OK but minimap Logic in another script
            if (inputManager.GetButtonDown("OpenMiniMap"))
            {
                if (miniMap.activeSelf) miniMap.SetActive(false);
                else miniMap.SetActive(true);
            }

        }
    }

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

            //Move Input
            moveInputVector.x = Input.GetAxis("Horizontal");
            moveInputVector.y = Input.GetAxis("Vertical");

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
