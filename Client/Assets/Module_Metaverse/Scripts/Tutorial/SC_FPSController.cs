using UnityEngine;
using Player;
namespace Tutorial
{
    [RequireComponent(typeof(CharacterController))]

    public class SC_FPSController : MonoBehaviour
    {
        /*-------------VARIABLES---------------*/
        [SerializeField] private float walkingSpeed = 7.5f;
        [SerializeField] private float runningSpeed = 11.5f;
        [SerializeField] private float jumpSpeed = 8.0f;
        [SerializeField] private float gravity = 20.0f;
        [SerializeField] private float lookSpeed = 2.0f;
        [SerializeField] private float lookXLimit = 45.0f;
        [SerializeField] private Camera playerCamera;

        [SerializeField] private GameObject infoText;


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

        public GameObject pauseMenu;

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
            //We get the character controller from the gameobject 
            characterController = GetComponent<CharacterController>();

            // Lock cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }

        void Update()
        {
            //A bit big??
            if (presentationCamera != null && (gameManager.TutorialStatus == TutorialStatus.Presentation || gameManager.TutorialStatus == TutorialStatus.Finished) && gameManager.DialogueStatus != DialogueStatus.InDialogue)
                playerUI.PresentationTextOn();

            if (raycastObject != null && gameManager.DialogueStatus != DialogueStatus.InDialogue)
                playerUI.EventTextOn();

            targetTime -= Time.deltaTime;
            //Raycast
            if (gameManager.TutorialStatus >= TutorialStatus.Interaction) PlayerRaycast();


            //K key down(PresentationMode)
            if (presentationCamera != null)
            {
                if (Input.GetKeyDown("k") && presentationCamera != null && (gameManager.TutorialStatus == TutorialStatus.Presentation || gameManager.TutorialStatus == TutorialStatus.Finished))
                {
                    if (gameManager.TutorialStatus == TutorialStatus.Presentation) triggerDetector.OnPresentation();

                    if (onPresentationCamera)
                    {
                        infoText.SetActive(true);
                        presentationCamera.enabled = false;
                        playerCamera.enabled = true;
                        if (gameManager.DialogueStatus != DialogueStatus.InDialogue)
                            playerUI.PresentationTextOn();
                        playerUI.ShowUI();
                    }

                    else
                    {
                        infoText.SetActive(false);
                        presentationCamera.enabled = true;
                        playerCamera.enabled = false;
                        playerUI.PresentationTextOff();
                        playerUI.HideUI();
                    }

                    onPresentationCamera = !onPresentationCamera;//Boolean cond modification always set to the opposite
                }
            }

            //Settings

            if (Input.GetKeyDown(KeyCode.Escape) && (gameManager.TutorialStatus == TutorialStatus.Finished))
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
                    //pauseMenu.GetComponent<PauseMenuSettingsTutorial>().StartTutorial();
                }

                this.enabled = false;

            }


            // We are grounded, so recalculate move direction based on axes


            //Refact from here
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

            //To here

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

        /// <summary>
        /// Changes between cameras.
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


        /// <summary>
        /// Does the raycast between the player and interactive items.
        /// </summary>
        public void PlayerRaycast()
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, rayDistance, LayerMask.GetMask("Interactive")))
            {
                //This executes if the raycast object from the former iteraction is null or the new one is different.
                if (raycastObject == null || raycastObject != hit.transform.gameObject)
                {
                    raycastObject = hit.transform.gameObject;
                    playerUI.EventTextOn();
                }

                //If the user interacts, activate the event
                if (Input.GetKey(KeyCode.E) && targetTime <= 0)
                {
                    //Cooldown timer
                    targetTime = 0.5f;

                    //Retrieve Parent Object and call event
                    GameObject eventObject = hit.transform.gameObject;
                    eventObject.GetComponent<IMetaEvent>().activate(true);

                    //Tutorial next step (Maybe change this?)
                    if (gameManager.TutorialStatus == TutorialStatus.Interaction) gameManager.CompleteObjective(0);
                }
            }

            //If the raycast don't detect an interactive item it removes the text
            else playerUI.EventTextOff();
        }

        /// <summary>
        /// Changes the GameStatus to Pause.
        /// </summary>
        public void Deactivate()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            playerUI.HideUI();
            gameManager.GameStatus = GameStatus.InPause;
            this.enabled = false;
        }


    }
}
