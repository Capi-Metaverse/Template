
using UnityEngine;



public class CharacterInputHandler : MonoBehaviour
{
    Vector2 moveInputVector = Vector2.zero;
    Vector2 viewInputVector = Vector2.zero;
    bool isJumpButtonPressed = false;


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
    
    //PlayerUIPrefab
    GameObject scope;
    GameObject micro;//Actually this is the microphone in game
    public GameObject eventText;
    public GameObject eventTextK;
    public GameObject changeRoomPanel = null;


    VoiceManager voiceChat = new VoiceManager();//Manager for the voiceChat, not in scene object
    CharacterMovementHandler characterMovementHandler;

    GameManager gameManager;
    InputManager inputManager;

    //Presentation
    public Camera presentationCamera = null;
    public bool onPresentationCamera = false;


    public string nickname;

    private void Awake()
    {
        gameManager = GameManager.FindInstance().GetComponent<GameManager>();
        inputManager = GameManager.FindInstance().GetComponent<InputManager>();

        
       
    }


    void Start()
    {
        voiceChat.GetGameObjects();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        voiceChat.recorder.TransmitEnabled = false;

        characterMovementHandler = this.gameObject.GetComponent<CharacterMovementHandler>();
        Settings = GameObject.Find("Menus").transform.GetChild(0).gameObject;
        Pause = GameObject.Find("Menus").transform.GetChild(1).gameObject;

        //PlayerUIPrefab
        //micro = GameObject.Find("PlayerUIPrefab").transform.GetChild(0).gameObject;//Micro
        //scope = GameObject.Find("PlayerUIPrefab").transform.GetChild(1).gameObject;//Scope
        //eventText = GameObject.Find("PlayerUIPrefab").transform.GetChild(2).gameObject;
        //eventTextK = GameObject.Find("PlayerUIPrefab").transform.GetChild(3).gameObject;
        //Debug.Log(GameManager.FindInstance().GetCurrentPlayer().transform.GetChild(3));
        micro = GameManager.FindInstance().GetCurrentPlayer().transform.GetChild(3).GetChild(0).gameObject;//Micro
        scope = GameManager.FindInstance().GetCurrentPlayer().transform.GetChild(3).GetChild(1).gameObject;//Scope
        eventText = GameManager.FindInstance().GetCurrentPlayer().transform.GetChild(3).GetChild(2).gameObject;
        eventTextK = GameManager.FindInstance().GetCurrentPlayer().transform.GetChild(3).GetChild(3).gameObject;
        Debug.Log(Pause);

        //seteamos el estado para que este InGame, esto hay que cambiarlo
        gameManager.SetUserStatus(UserStatus.InGame);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("m") && gameManager.GetUserStatus() == UserStatus.InGame)
            voiceChat.MuteAudio(gameManager.GetUserStatus());
        nickname = this.gameObject.GetComponent<NetworkPlayer>().nickname.ToString();

       
        if (localCameraHandler == null) {
            
            localCameraHandler = GetComponentInChildren<LocalCameraHandler>();
            playerCamera = localCameraHandler.gameObject.GetComponent<Camera>();

        }

     
            if (HittingObject && gameManager.GetUserStatus() != UserStatus.InPause && onPresentationCamera==false)
                eventText.SetActive(true);

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

            //Pause
            if (!Input.GetKeyDown(KeyCode.Escape))
            {

                escPul = false; // Detecta si no esta pulsado

            }

            switch (gameManager.GetUserStatus())
            {
                case UserStatus.InGame:
                    {
                        //ESC key down(PauseMenu)
                        if ((Input.GetKeyDown(KeyCode.Escape) && !escPul))
                        {
                            setPause();
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
                                }

                                else
                                {
                                    presentationCamera.enabled = true;
                                    playerCamera.enabled = false;
                                    eventText.SetActive(false);
                                    DeactivateALL();

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
                        if(changeRoomPanel != null)
                          changeRoomPanel.SetActive(false);

}
                        break;
                    }

                default:
                    Debug.Log(gameManager.GetUserStatus());
                    break;
            }


            //View input
            viewInputVector.x = Input.GetAxis("Mouse X");
            viewInputVector.y = Input.GetAxis("Mouse Y") * -1; //Invert the mouse look

            //Move Input
            moveInputVector.x = Input.GetAxis("Horizontal");
            moveInputVector.y = Input.GetAxis("Vertical");

            if (Input.GetButton("Jump"))
                isJumpButtonPressed = true;

            if (localCameraHandler != null) localCameraHandler.SetViewInputVector(viewInputVector);
        }
    

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

    //DeactivateALL
    public void DeactivateALL()
    {
        escPul = true;
        gameManager.SetUserStatus(UserStatus.InPause);
        
    //playerToSpawn.GetComponent<SC_FPSController>().enabled = false;


    Cursor.visible = true;
        //AQUI IRA EL FIND DEL CHARACTER CONTROL PARA DESACTIVAR
        //AQUI IRA EL FIND DEL PLAYERCAMERA PARA DESACTIVARLA
        characterMovementHandler.enabled=false;
       
        localCameraHandler.enabled=false;


        //Deactivate presentation text
        eventTextK.SetActive(false);
        eventText.SetActive(false);

        micro.SetActive(false);
        scope.SetActive(false);
        Cursor.lockState = CursorLockMode.None; // Desactiva el bloqueo cursor
    }

    //ActiveALL
    public void ActiveALL()
    {

        //playerToSpawn.GetComponent<SC_FPSController>().enabled = true;



        // AQUI IRA EL FIND DEL CHARACTER CONTROL PARA ACTIVAR
        //AQUI IRA EL FIND DEL PLAYERCAMERA PARA ACTIVAR
        characterMovementHandler.enabled = true;
        localCameraHandler.enabled = true;


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
    

    public void setJuego()
    {

        //Activate Settings Window and stop

        Settings.SetActive(false);
        Pause.SetActive(false);
        Cursor.visible = false;
        //States and Reactivate all
      
        gameManager.SetUserStatus(UserStatus.InGame);
        ActiveALL();
        Debug.Log(gameManager.GetUserStatus());

    }

 
}
