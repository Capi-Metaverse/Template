using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


[RequireComponent(typeof(CharacterController))]

public class SC_FPSController : MonoBehaviour
{
    public TMP_Text playerNameGame;
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public float sensitivity;
    public Sprite imagenPrueba;

    private bool isFalling;

    private bool isRunning;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    //Raycast distance
    public float rayDistance = 3;
    public bool active = false;

    //GameManager
    public PlayerSpawn playerSpawner;
    public float targetTime = 0.5f;

    GameObject raycastObject = null;

    [HideInInspector]
    public bool canMove = true;


    InputManager inputManager;

    GameObject eventText;

    //Animations
    Animator anim;
    float SpeedAnim;
    bool WalkingAnim;

    //PhotonEvents
    object[] content;

    void Start()
    {
        inputManager = GameObject.FindObjectOfType<InputManager>();
        characterController = GetComponent<CharacterController>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        playerSpawner = GameObject.Find("PlayerSpawner").GetComponent<PlayerSpawn>();

        eventText = GameObject.Find("PlayerUIPrefab").transform.GetChild(2).gameObject;
        
        Debug.Log(eventText);
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
        sensitivity = PlayerPrefs.GetFloat("Sensitivity",1.0f);
        targetTime -= Time.deltaTime;
          //Raycast
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, rayDistance, LayerMask.GetMask("Interactive")))
        {
            if(raycastObject == null){
            raycastObject = hit.transform.gameObject;
            //raycastObject.gameObject.GetComponent<Outline>().enabled = true;
          
           
           eventText.SetActive(true);

           
            }
            //RaycastObject
            else if(raycastObject != hit.transform.gameObject ){
            //raycastObject.GetComponent<Outline>().enabled = false;
            raycastObject = hit.transform.gameObject;
            //hit.transform.gameObject.GetComponent<Outline>().enabled = true;
            eventText.SetActive(true);
            }
            //If the user interacts, activate the event
            if (inputManager.GetButtonDown("Interact") && targetTime <=0)
            {
                //Cooldown timer
                targetTime=0.5f;

                //Retrieve Parent Object and call event
                GameObject eventObject = hit.transform.gameObject;
                
                eventObject.GetComponent<IMetaEvent>().activate(true);
            }
        }

        else{

             if(raycastObject != null){
                //raycastObject.GetComponent<Outline>().enabled = false;
                raycastObject = null;
                eventText.SetActive(false);
            }
        }


        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
            anim.SetTrigger("Jumping");
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        //Determine Animation State
        if ((Input.GetAxis("Vertical")!= 0)|| (Input.GetAxis("Horizontal")!=0)){

            WalkingAnim = true;

            if (Input.GetAxis("Vertical")>0){
                SpeedAnim = 1.0f;
            } else if (Input.GetAxis("Vertical")<0){
                SpeedAnim = -1.0f;
            }
       
        } else {
            WalkingAnim = false;
        }

        // Update local Animations
        anim.SetBool("Walking",WalkingAnim);
        anim.SetFloat("Speed",SpeedAnim);
        anim.SetBool("Running",isRunning);

        //Update Remote Animations
        UpdateAnimations();

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed  / sensitivity;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX , 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed / sensitivity, 0);
        }
        //Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * rayDistance, Color.red );
    } 

    /// <summary>
    /// Updates the Animation State in the screen of other players 
    /// </summary>
    void UpdateAnimations(){
            //Update Walking for other Players
            content = new object[] {
                        transform.GetComponent<PhotonView>().Owner.NickName,
                        WalkingAnim,
                        "Walking"
                    };
            RaiseEventAnimation(content);

            //Update Speed for other Players
            content = new object[] {
                        transform.GetComponent<PhotonView>().Owner.NickName,
                        SpeedAnim,
                        "SpeedAnim"
                    };
            RaiseEventAnimation(content);

            //Update Running for other Players
            content = new object[] {
                        transform.GetComponent<PhotonView>().Owner.NickName,
                        isRunning,
                        "Running"
                    };
            RaiseEventAnimation(content);
    }
    void RaiseEventAnimation(object[] content){
        RaiseEventOptions raiseEventOptions =
                    new RaiseEventOptions { Receivers = ReceiverGroup.Others }; // You would have to set the Receivers to All in order to receive this event on the local client as well
                PhotonNetwork
                    .RaiseEvent(2,
                    content,
                    raiseEventOptions,
                    SendOptions.SendReliable);
    }
}
