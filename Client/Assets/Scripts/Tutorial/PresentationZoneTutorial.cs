using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PresentationZoneTutorial : MonoBehaviour
{
    //Camera of the presentation
    public Camera cameraObject;

    SC_FPSController playerInputs;


    //Detect if it in collider
    private void OnTriggerEnter(Collider other)
    {
        playerInputs = GameObject.FindGameObjectWithTag("Player").GetComponent<SC_FPSController>();
        playerInputs.setPresentationCamera(cameraObject);
    }
    //Detect if Exits collider
    private void OnTriggerExit(Collider other)
    {
        playerInputs = GameObject.FindGameObjectWithTag("Player").GetComponent<SC_FPSController>();
        playerInputs.setPresentationCamera(null);
    }
}
