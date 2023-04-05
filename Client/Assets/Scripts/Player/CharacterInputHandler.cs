
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
    public GameObject eventText;
    public GameObject eventTextK;
    //Detect if Certain Object is being hit
    bool HittingObject = false;
    public Camera playerCamera;

    LocalCameraHandler localCameraHandler;
    public bool escPul;//Reference if ESC key is pushed or not(ESC opens the Menu and youï¿½ll be on Pause State)
    UserStatus estado; //With this we keep track of the current state so we can use it in conditionals. States are (Game, Pause)
    public GameObject Pause;//Pause is an object in scene map, you can see it as the manager of the pause state
    public GameObject Settings;//The same as Pause but for settings, the state will be Pause too cause the setting are accesible from Pause
    GameObject scope;
    public GameObject micro;//Actually this is the microphone in game
    private VoiceManager voiceChat = new VoiceManager();//Manager for the voiceChat, not in scene object
    CharacterMovementHandler characterMovementHandler;

    private InputManager inputManager;
    

    private void Awake()
    {
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
        micro = GameObject.Find("PlayerUIPrefab").transform.GetChild(0).gameObject;//Micro
        scope = GameObject.Find("PlayerUIPrefab").transform.GetChild(1).gameObject;//Scope
        eventText = GameObject.Find("PlayerUIPrefab").transform.GetChild(2).gameObject;
        eventTextK = GameObject.Find("PlayerUIPrefab").transform.GetChild(3).gameObject;

        eventText = GameObject.Find("PlayerUIPrefab").transform.GetChild(2).gameObject;
        Debug.Log(Pause);

        //seteamos el estado para que este InGame, esto hay que cambiarlo
        estado = UserStatus.InGame;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("m") && estado == UserStatus.InGame)
            voiceChat.MuteAudio(estado);

        if (localCameraHandler == null) { 
            localCameraHandler = GetComponentInChildren<LocalCameraHandler>();
            playerCamera = localCameraHandler.gameObject.GetComponent<Camera>();

        }

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
                Debug.Log("Activado");
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

            escPul = false; // Detecta si no estï¿½ pulsado

        }

        switch (estado)
        {
            case UserStatus.InGame:
                {
                    //ESC key down(PauseMenu)
                    if (Input.GetKeyDown(KeyCode.Escape) && !escPul)
                    {
                        setPause();
                    }
                    break;
                }
            case UserStatus.InPause:
                {
                    if (Input.GetKeyDown(KeyCode.Escape) && !escPul)
                    {
                        setJuego();
                    }
                    break;
                }

            default:
                Debug.Log(estado);
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

    //DeactivateALL
    public void DeactivateALL()
    {

        //playerToSpawn.GetComponent<SC_FPSController>().enabled = false;


        Cursor.visible = true;
        //AQUI IRA EL FIND DEL CHARACTER CONTROL PARA DESACTIVAR
        //AQUI IRA EL FIND DEL PLAYERCAMERA PARA DESACTIVARLA
        characterMovementHandler.enabled=false;
        localCameraHandler.enabled=false;


        //Deactivate presentation text
        if (eventTextK != null)
        {
            eventTextK.SetActive(false);
        }

        ///DESACTIVAR LAS LETRAS DE PULSA E
        if (eventText != null)
        {
            eventText.SetActive(false);
        }


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
        if (eventTextK != null)
        {
            eventTextK.SetActive(true);
        }

        ///DESACTIVAR LAS LETRAS DE PULSA E
        if (eventText != null)
        {
            eventText.SetActive(true);
        }


        Cursor.visible = false;
        scope.SetActive(true);
        micro.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked; // Desactiva el bloqueo cursor 
    }

    public void setPause()
    {
        escPul = true;
        //playerToSpawn.GetComponent<SC_FPSController>().eventText.SetActive(false);




        //Start Animator
        //animator.speed = 0;
        //object[] content =new object[] {playerToSpawn.GetComponent<PhotonView>().Owner.NickName,animator.speed,"Stop&Replay"};
        //RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        //PhotonNetwork.RaiseEvent(2,content,raiseEventOptions,SendOptions.SendReliable);



        //Pause canvas
        estado = UserStatus.InPause;
        Pause.SetActive(true);
        //RoomName on Settings
        //GameObject SalaText = Pausa.transform.Find("RoomName").gameObject;
        //Scene scene = SceneManager.GetActiveScene();
        //SalaText.GetComponent<TMP_Text>().text = ((string) PhotonNetwork.CurrentRoom.CustomProperties["Name"]) + " " + scene.name;
        //State and Cursor
       
        DeactivateALL();
        //Escape activado
        //UnityEngine.Debug.Log (estado);



    }
    

    public void setJuego()
    {

        //Activate Settings Window and stop

        Settings.SetActive(false);
        Pause.SetActive(false);
        Cursor.visible = false;
        //States and Reactivate all
      
        estado = UserStatus.InGame;
        ActiveALL();
        Debug.Log(estado);

    }

 
}
