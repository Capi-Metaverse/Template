using UnityEngine;
using TMPro;
using static Unity.Collections.Unicode;


[RequireComponent(typeof(CharacterController))]

public class SC_FPSController : MonoBehaviour
{
    /*-------------VARIABLES---------------*/
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    private int jumpCount { get; set; } = 0;
    private int _lastVisibleJump = 0;
    public bool IsGrounded { get; set; }
    public Vector3 Velocity { get; set; }

    //Add sensitivity
    public float sensitivity;

    public TriggerDetector triggerDetector;
  
    private bool isRunning;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    //Raycast distance
    public float rayDistance = 3;

   
    public float targetTime = 0.5f;
    GameObject raycastObject = null;
    [HideInInspector]
    public bool canMove = true;

    InputManager inputManager;

    public GameObject pauseMenu;

    //Detect if Certain Object is being hit
    bool HittingObject = false;

    //Presentation
    public Camera presentationCamera = null;
    public bool onPresentationCamera = false;

    //PlayerUIPrefab
    [SerializeField] public PlayerUI playerUI;

    public Animator animator;
    public GameManagerTutorial gameManager;


    /*-----------------------METHODS------------------------------*/
    void Start()
    {
        //gameManager = GameObject.Find("ManagerTutorial").GetComponent<GameManagerTutorial>();
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void Update()
    {
        if (presentationCamera != null && (gameManager.TutorialStatus == TutorialStatus.Presentation || gameManager.TutorialStatus == TutorialStatus.Finished) && gameManager.DialogueStatus!=DialogueStatus.InDialogue)
            playerUI.PresentationTextOn();

        if (HittingObject && gameManager.DialogueStatus != DialogueStatus.InDialogue)
            playerUI.EventTextOn();

        targetTime -= Time.deltaTime;
        //Raycast
        if (gameManager.TutorialStatus >= TutorialStatus.Interaction)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, rayDistance, LayerMask.GetMask("Interactive")))
            {
                if (raycastObject == null)
                {
                    raycastObject = hit.transform.gameObject;
                    //raycastObject.gameObject.GetComponent<Outline>().enabled = true;
                    playerUI.EventTextOn();
                    HittingObject = true;
                }
                //RaycastObject
                else if (raycastObject != hit.transform.gameObject)
                {

                    raycastObject = hit.transform.gameObject;

                    playerUI.EventTextOn();
                    HittingObject = true;

                }
                //If the user interacts, activate the event
                if (Input.GetKey(KeyCode.E) && targetTime <= 0)
                {
                    //Cooldown timer
                    targetTime = 0.5f;

                    //Retrieve Parent Object and call event
                    GameObject eventObject = hit.transform.gameObject;
                    eventObject.GetComponent<IMetaEvent>().activate(true);
                    if (gameManager.TutorialStatus == TutorialStatus.Interaction) triggerDetector.endInteraction();
                }
            }

            else
            {

                if (raycastObject != null)
                {
                    //raycastObject.GetComponent<Outline>().enabled = false;
                    raycastObject = null;
                    playerUI.EventTextOff();
                    HittingObject = false;
                }
            }
        }

        //K key down(PresentationMode)
        if (presentationCamera != null)
        {
            if (Input.GetKeyDown("k") && presentationCamera != null && (gameManager.TutorialStatus == TutorialStatus.Presentation || gameManager.TutorialStatus == TutorialStatus.Finished))
            {
                if (gameManager.TutorialStatus == TutorialStatus.Presentation) triggerDetector.OnPresentation();

                if (onPresentationCamera)
                {
                    presentationCamera.enabled = false;
                    playerCamera.enabled = true;
                    if (gameManager.DialogueStatus!=DialogueStatus.InDialogue)
                        playerUI.PresentationTextOn();
                    playerUI.ShowUI();
                }

                else
                {
                    presentationCamera.enabled = true;
                    playerCamera.enabled = false;
                    playerUI.PresentationTextOff();
                    playerUI.HideUI();
                }

                onPresentationCamera = !onPresentationCamera;//Boolean cond modification always set to the opposite
            }
        }

        //Settings
        if (Input.GetKeyDown(KeyCode.Escape) && (gameManager.TutorialStatus == TutorialStatus.PreSettings))
        {

            //Open pause menu and disable this
            gameManager.GameStatus = GameStatus.InPause;

            pauseMenu.SetActive(true);
            playerUI.HideUI();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (gameManager.TutorialStatus == TutorialStatus.PreSettings)
            {
                gameManager.TutorialStatus = TutorialStatus.Settings;
                pauseMenu.GetComponent<PauseMenuSettingsTutorial>().StartTutorial();
            }

            this.enabled = false;

        }

        // We are grounded, so recalculate move direction based on axes
        var previousPos = transform.position;
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

        Velocity = (transform.position - previousPos);
        IsGrounded = characterController.isGrounded;
        if (IsGrounded) jumpCount++;

        //Animations
        if (animator == null) { animator = this.gameObject.GetComponentInChildren<Animator>(); }

        if (IsGrounded)
        {

            if (Velocity.magnitude > 0 && IsGrounded)
            {

                animator.SetBool("Walking", true);

            }
            else
            {
                animator.SetBool("Walking", false);
            }
            animator.SetBool("Running", isRunning);
        }

        else
        {
            if (jumpCount > _lastVisibleJump)
            {
                animator.SetTrigger("Jumping");
                // Play jump sound/particle effect
                _lastVisibleJump = jumpCount;
            }

        }

    }

    public void setPresentationCamera(Camera camera)
    {

        //When leaving presentation mode?
        if (camera == null)
        {
            //We deactivate the camera
            presentationCamera = null;

            //We deactivate UI
            playerUI.PresentationTextOff();
        }

        else
        {
            //We obtain the camera
            presentationCamera = camera;
            //Debug.Log(gameManager.TutorialStatus);
            //We activate UI
            if (gameManager.TutorialStatus == TutorialStatus.Presentation || gameManager.TutorialStatus == TutorialStatus.Finished)
                playerUI.PresentationTextOn();
        }
    }


}