using System;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[OrderBefore(typeof(NetworkTransform))]
[DisallowMultipleComponent]
// ReSharper disable once CheckNamespace
public class NetworkCharacterControllerPrototypeCustom : NetworkTransform {
  [Header("Character Controller Settings")]
  private float gravity       = -20.0f;
    private float jumpImpulse   = 4.0f;
    private float acceleration  = 100.0f;
    private float braking       = 50.0f;//how much vel is decremented when stopped moving
    private float maxSpeed      = 3.0f;
    private float rotationSpeed = 50.0f;
    public float viewUpDownRotationSpeed = 50.0f;
    //Run
    public float Speed;
    public float RunSpeed = 3.0f;
    public float NormalSpeed = 1.0f;
    public bool isRunning = false;

    [Networked]
  [HideInInspector]
  public bool IsGrounded { get; set; }

  [Networked]
  [HideInInspector]
  public Vector3 Velocity { get; set; }

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

  protected override void Awake() {
    base.Awake();
    CacheController();
  }

  public override void Spawned() {
    base.Spawned();
    CacheController();
  }

  private void CacheController() {
    if (Controller == null) {
      Controller = GetComponent<CharacterController>();

      Assert.Check(Controller != null, $"An object with {nameof(NetworkCharacterControllerPrototype)} must also have a {nameof(CharacterController)} component.");
    }
  }

  protected override void CopyFromBufferToEngine() {
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
  public virtual void Jump(bool ignoreGrounded = false, float? overrideImpulse = null) {
    if (IsGrounded || ignoreGrounded) {
      var newVel = Velocity;
      newVel.y += overrideImpulse ?? jumpImpulse;
      Velocity =  newVel;
    }
  }

  /// <summary>
  /// Basic implementation of a character controller's movement function based on an intended direction.
  /// <param name="direction">Intended movement direction, subject to movement query, acceleration and max speed values.</param>
  /// </summary>
  public virtual void Move(Vector3 direction) {
    var deltaTime    = Runner.DeltaTime;
    var previousPos  = transform.position;
    var moveVelocity = Velocity;

    //If is in the ground and his velocity on Y is negative it means that keeps falling so we put velocity(Y) to 0.
    if (IsGrounded && moveVelocity.y < 0) {
      direction = Transform.up * 0;//Stop falling when touch the ground
    }

    if (Input.GetKey(KeyCode.LeftShift))
    {
        isRunning = true;
        Speed = RunSpeed;
        print("Running");

    }
    else
    {
        isRunning = false;
        Speed = NormalSpeed;
        print("Not Running");
    }

    if (Input.GetKey("w"))
        direction += Transform.forward * Speed;
    if (Input.GetKey("s"))
        direction += Transform.forward * -Speed;
    if (Input.GetKey("a"))
        direction += Transform.right * -Speed;
    if (Input.GetKey("d"))
        direction += Transform.right * Speed;

    direction += Transform.up * gravity;

    if (Input.GetKeyDown(KeyCode.Space))
    {
        direction -= Transform.up * 10f;
    }

    Controller.Move(direction * deltaTime);

    Velocity   = (transform.position - previousPos) * Runner.Simulation.Config.TickRate;
    IsGrounded = Controller.isGrounded;
  }

    public void Rotate(float rotationY)
    {
        transform.Rotate(0, rotationY * Runner.DeltaTime * rotationSpeed, 0);
    }
}