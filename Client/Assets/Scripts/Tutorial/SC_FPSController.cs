
using UnityEngine;

using TMPro;


[RequireComponent(typeof(CharacterController))]

public class SC_FPSController : MonoBehaviour
{
    /*-------------VARIABLES---------------*/
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
    private bool isWaving;
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    //Raycast distance
    public float rayDistance = 3;
    public bool active = false;

    public float targetTime = 0.5f;
    GameObject raycastObject = null;
    [HideInInspector]
    public bool canMove = true;
    InputManager inputManager;

    public GameObject eventText;

    //Detect if Certain Object is being hit
    bool HittingObject = false;

    /*-----------------------METHODS------------------------------*/
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
       

        if (HittingObject)
            eventText.SetActive(true);

        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 1.0f);
        targetTime -= Time.deltaTime;
        //Raycast
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, rayDistance, LayerMask.GetMask("Interactive")))
        {
            if (raycastObject == null)
            {
                raycastObject = hit.transform.gameObject;
                //raycastObject.gameObject.GetComponent<Outline>().enabled = true;
                eventText.SetActive(true);
                HittingObject = true;
            }
            //RaycastObject
            else if (raycastObject != hit.transform.gameObject)
            {
                //raycastObject.GetComponent<Outline>().enabled = false;
                raycastObject = hit.transform.gameObject;
                //hit.transform.gameObject.GetComponent<Outline>().enabled = true;
                eventText.SetActive(true);
                HittingObject = true;

            }
            //If the user interacts, activate the event
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

        

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed / sensitivity;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed / sensitivity, 0);
        }

    }

    
}