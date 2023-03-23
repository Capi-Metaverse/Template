using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterInputHandler : MonoBehaviour
{
    Vector2 moveInputVector = Vector2.zero;
    Vector2 viewInputVector = Vector2.zero;
    bool isJumpButtonPressed = false;

    LocalCameraHandler localCameraHandler;

    private void Awake()
    {
        
    }


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(localCameraHandler == null) localCameraHandler = GetComponentInChildren<LocalCameraHandler>();

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
}
