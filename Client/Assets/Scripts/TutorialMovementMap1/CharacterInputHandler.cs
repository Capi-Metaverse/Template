using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Pseudo;
using UnityEngine.SceneManagement;

public class CharacterInputHandler : MonoBehaviour
{
    Vector2 moveInputVector = Vector2.zero;
    Vector2 viewInputVector = Vector2.zero;
    bool isJumpButtonPressed = false;

    LocalCameraHandler localCameraHandler;
    public bool escPul;//Reference if ESC key is pushed or not(ESC opens the Menu and you´ll be on Pause State)
    UserStatus estado; //With this we keep track of the current state so we can use it in conditionals. States are (Game, Pause)
    public GameObject Pause;//Pause is an object in scene map, you can see it as the manager of the pause state
    public GameObject Settings;//The same as Pause but for settings, the state will be Pause too cause the setting are accesible from Pause

    CharacterMovementHandler characterMovementHandler;


    private void Awake()
    {
        
    }


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Settings = GameObject.Find("Menus").transform.GetChild(0).gameObject;
        Pause = GameObject.Find("Menus").transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(localCameraHandler == null) localCameraHandler = GetComponentInChildren<LocalCameraHandler>();

        //Pause
        if (!Input.GetKeyDown(KeyCode.Escape))
        {
            escPul = false; // Detecta si no está pulsado
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

        Cursor.lockState = CursorLockMode.None; // Desactiva el bloqueo cursor
    }

    //ActiveALL
    public void ActiveALL()
    {

        Cursor.visible = false;

        Cursor.lockState = CursorLockMode.Locked; // Desactiva el bloqueo cursor 
    }

    public void setPause()
    {

        escPul = true;


        //Pause canvas

        Pause.SetActive(true);
        //RoomName on Settings
        Scene scene = SceneManager.GetActiveScene();
        //State and Cursor
        estado = UserStatus.InPause;
        Cursor.visible = true;
        DeactivateALL();
        //Escape activado
        Debug.Log(estado);

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
