using System;
using Fusion;
using Photon.Realtime;
using UnityEngine;
using Manager;

// ReSharper disable once CheckNamespace
namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    [OrderBefore(typeof(NetworkTransform))]
    [DisallowMultipleComponent]
    public class NetworkCharacterControllerPrototypeCustom : NetworkTransform
    {
        [Header("Character Controller Settings")]

        private float jumpImpulse = 4.0f;
        private float rotationSpeed = 50.0f;
        public float viewUpDownRotationSpeed = 50.0f;
        //Run
        public bool canMove = true;
        [Networked]
        private bool isFalling { get; set; }
        [Networked]
        private bool isRunning { get; set; }

        [Networked]
        private int jumpCount { get; set; } = 0;
        private int _lastVisibleJump = 0;
        public float walkingSpeed = 3.5f;
        public float runningSpeed = 5.5f;
        public float jumpSpeed = 4.0f;
        [SerializeField] private float gravity = 55.0f;
        public float lookSpeed = 2.0f;
        public float lookXLimit = 45.0f;
        public float sensitivity = 10.0f;
        Vector3 moveDirection = Vector3.zero;
        float rotationX = 0;

        public Animator animator = null;
        public PhotonManager photonManager = null;

        [Networked]
        [HideInInspector]
        public bool IsGrounded { get; set; }

        [Networked]
        [HideInInspector]
        public Vector3 Velocity { get; set; }

        [Networked]
        public bool IsPaused { get; set; } = false;

        //Managers

        private PauseManager pauseManager;

        /// <summary>
        /// Sets the default teleport interpolation velocity to be the CC's current velocity.
        /// For more details on how this field is used, see <see cref="NetworkTransform.TeleportToPosition"/>.
        /// </summary>
        protected override Vector3 DefaultTeleportInterpolationVelocity => Velocity;

        /// <summary>
        /// Sets the default teleport interpolation angular velocity to be the CC's rotation speed on the Z axis.
        /// For more details on how this field is used, see <see cref="NetworkTransform.TeleportToRotation"/>.
        /// </summary>
        protected override Vector3 DefaultTeleportInterpolationAngularVelocity => new Vector3(0f, 0f, rotationSpeed);

        public CharacterController Controller { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            CacheController();
            photonManager = PhotonManager.FindInstance();
            pauseManager = PauseManager.FindInstance();


        }

        public override void Spawned()
        {
            base.Spawned();
            CacheController();

            if (animator == null) { animator = this.gameObject.GetComponentInChildren<Animator>(); }
        }

        private void CacheController()
        {
            if (Controller == null)
            {
                Controller = GetComponent<CharacterController>();

                //Assert.Check(Controller != null, $"An object with {nameof(NetworkCharacterControllerPrototype)} must also have a {nameof(CharacterController)} component.");
            }
        }

        protected override void CopyFromBufferToEngine()
        {
            // Trick: CC must be disabled before resetting the transform state
            Controller.enabled = false;

            // Pull base (NetworkTransform) state from networked data buffer
            base.CopyFromBufferToEngine();

            // Re-enable CC
            Controller.enabled = true;
        }

        /// <summary>
        /// Basic implementation of a jump impulse (immediately integrates a vertical component to Velocity).
        /// <param name="ignoreGrounded">Jump even if not in a grounded state.</param>
        /// <param name="overrideImpulse">Optional field to override the jump impulse. If null, <see cref="jumpImpulse"/> is used.</param>
        /// </summary>
        public virtual void Jump(bool ignoreGrounded = false, float? overrideImpulse = null)
        {
            if (IsGrounded || ignoreGrounded)
            {
                var newVel = Velocity;
                newVel.y += overrideImpulse ?? jumpImpulse;
                Velocity = newVel;
            }
        }

        /// <summary>
        /// Basic implementation of a character controller's movement function based on an intended direction.
        /// <param name="direction">Intended movement direction, subject to movement query, acceleration and max speed values.</param>
        /// </summary>
        public virtual void Move(Vector3 direction)
        {
            var deltaTime = Runner.DeltaTime;
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

            if (Input.GetButton("Jump") && canMove && Controller.isGrounded)
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
            if (!Controller.isGrounded)
            {
                moveDirection.y -= gravity * deltaTime;
            }

            direction = moveDirection;

            Controller.Move(direction * deltaTime);

            if (canMove)
            {
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed / sensitivity;

                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed / sensitivity, 0);
            }

            Velocity = (transform.position - previousPos) * Runner.Simulation.Config.TickRate;
            IsGrounded = Controller.isGrounded;
            if (IsGrounded) jumpCount++;

        }

        public void Rotate(float rotationY)
        {
            transform.Rotate(0, rotationY * Runner.DeltaTime * rotationSpeed, 0);
        }


        public override void Render()
        {

            if (IsPaused)
            {

                //animator.SetFloat("Pause", 1); 
                animator.SetBool("Walking", false);
                animator.SetBool("Running", false);

            }
            //else { animator.SetFloat("Pause", 0); }
            else
            {
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



        }


    }
}