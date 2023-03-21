using System.Collections;
using System.Collections.Generic;
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
    
}