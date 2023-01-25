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

    public Sprite imagenPrueba;

    
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    //Raycast distance
    public float rayDistance = 3;
    public bool active = false;

    //GameManager
    public PlayerSpawn playerSpawner;
    public float targetTime = 0.5f;

    [HideInInspector]
    public bool canMove = true;

    Animator anim;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        playerSpawner = GameObject.Find("PlayerSpawner").GetComponent<PlayerSpawn>();
      
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {

        targetTime -= Time.deltaTime;
          //Raycast
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, rayDistance, LayerMask.GetMask("Interactive")))
        {
            //If the user interacts, activate the event
            if (Input.GetButton("Interact") && targetTime <=0)
            {
                //Cooldown timer
                targetTime=0.5f;

                //Retrieve Parent Object and call event
                GameObject eventObject = hit.transform.gameObject;
                eventObject.GetComponent<IMetaEvent>().activate(true);
            }
        }



        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
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

        //Animation
        if ((Input.GetAxis("Vertical")!= 0)|| (Input.GetAxis("Horizontal")!=0))
            anim.SetBool("Walking",true);
        else
            anim.SetBool("Walking",false);
        anim.SetBool("Running",isRunning);

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
        //Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * rayDistance, Color.red );
    } 
}
