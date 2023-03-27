
using UnityEngine;



public class CharacterInputHandler : MonoBehaviour
{
    Vector2 moveInputVector = Vector2.zero;
    Vector2 viewInputVector = Vector2.zero;
    bool isJumpButtonPressed = false;

    LocalCameraHandler localCameraHandler;
    public bool escPul;//Reference if ESC key is pushed or not(ESC opens the Menu and youï¿½ll be on Pause State)
    UserStatus estado; //With this we keep track of the current state so we can use it in conditionals. States are (Game, Pause)
    public GameObject Pause;//Pause is an object in scene map, you can see it as the manager of the pause state
    public GameObject Settings;//The same as Pause but for settings, the state will be Pause too cause the setting are accesible from Pause
    GameObject scope;
    CharacterMovementHandler characterMovementHandler;
    

    private void Awake()
    {
        
    }


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        characterMovementHandler = this.gameObject.GetComponent<CharacterMovementHandler>();
        Settings = GameObject.Find("Menus").transform.GetChild(0).gameObject;
        Pause = GameObject.Find("Menus").transform.GetChild(1).gameObject;
        scope = GameObject.Find("PlayerUIPrefab").transform.GetChild(1).gameObject;//Scope
        Debug.Log(Pause);

        //seteamos el estado para que este InGame, esto hay que cambiarlo
        estado = UserStatus.InGame;
    }

    // Update is called once per frame
    void Update()
    {
        if(localCameraHandler == null) localCameraHandler = GetComponentInChildren<LocalCameraHandler>();

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



        //AQUI IRA EL FIND DEL CHARACTER CONTROL PARA DESACTIVAR
        //AQUI IRA EL FIND DEL PLAYERCAMERA PARA DESACTIVARLA
        characterMovementHandler.enabled=false;
        localCameraHandler.enabled=false;


        //Deactivate presentation text
        /*if (eventTextK != null) 
        {v
            eventTextK.SetActive(false);
        }*/

        ///DESACTIVAR LAS LETRAS DE PULSA E




         scope.SetActive(false);
        // micro.SetActive(false);
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
        /*if (eventTextK != null) 
        {
            eventTextK.SetActive(true);
        }*/

        ///DESACTIVAR LAS LETRAS DE PULSA E



        Cursor.visible = false;
        scope.SetActive(true);
        //micro.SetActive(true);
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

        Pause.SetActive(true);
        //RoomName on Settings
        //GameObject SalaText = Pausa.transform.Find("RoomName").gameObject;
        //Scene scene = SceneManager.GetActiveScene();
        //SalaText.GetComponent<TMP_Text>().text = ((string) PhotonNetwork.CurrentRoom.CustomProperties["Name"]) + " " + scene.name;
        //State and Cursor
        estado = UserStatus.InPause;
        Cursor.visible = true;  
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
