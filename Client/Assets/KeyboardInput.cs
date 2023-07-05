using System.Collections;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeyboardInput : MonoBehaviour
{

    public bool canMove = true;
    public Vector3 MoveDirection { get; private set; }

    private Camera _cam;

    


    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        MoveDirection = Vector3.zero;
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.S);

        if (Input.GetKey(KeyCode.W)) MoveDirection += _cam.transform.forward;
        if (Input.GetKey(KeyCode.S)) MoveDirection -= _cam.transform.forward;
        if (Input.GetKey(KeyCode.A)) MoveDirection += _cam.transform.right;
        if (Input.GetKey(KeyCode.D)) MoveDirection -= _cam.transform.right;

    }
}
