using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;



public class SC_FPSController : NetworkBehaviour
{
    /*-------------VARIABLES---------------*/
    public TMP_Text playerNameGame;
    public float walkingSpeed = 3.5f;
    public float runningSpeed = 5.5f;
    public float jumpSpeed = 4.0f;
    public float gravity = 30.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public float sensitivity = 1.0f;
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
   
    public float targetTime = 0.5f;
    GameObject raycastObject = null;
    [HideInInspector]
    public bool canMove = true;


    public GameObject eventText;

    //Animations
    Animator anim;
    float SpeedAnim;
    bool WalkingAnim;


    object[] content;

    //Detect if Certain Object is being hit
    bool HittingObject = false;

    /*-----------------------METHODS------------------------------*/

    private void Awake()
    {
       characterController =  this.gameObject.GetComponentInParent<CharacterController>();
       playerCamera = this.gameObject.transform.GetChild(1).gameObject.GetComponent<Camera>();
       Cursor.lockState = CursorLockMode.Locked;
    }


    public void Update() 
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        isRunning = Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.S);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        //Normalize diagonal speed
        moveDirection = moveDirection.normalized * Mathf.Clamp(moveDirection.magnitude, 0, 50);//Parameters are:(Value,MinX,MaxX)

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
            //anim.SetTrigger("Jumping");
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
        if ((Input.GetAxis("Vertical") != 0) || (Input.GetAxis("Horizontal") != 0))
        {

            WalkingAnim = true;

            if (Input.GetAxis("Vertical") > 0)
            {
                SpeedAnim = 1.0f;
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                SpeedAnim = -1.0f;
            }

        }
        else
        {
            WalkingAnim = false;
        }

        // Update local Animations
        //anim.SetBool("Walking", WalkingAnim);
        //anim.SetFloat("Speed", SpeedAnim);
        //anim.SetBool("Running", isRunning);

        //Update Remote Animations
        //UpdateAnimations();

        // Player and Camera rotation
        
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed / sensitivity;
         
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed / sensitivity, 0);
        }
        
        //Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * rayDistance, Color.red );
    }
}
