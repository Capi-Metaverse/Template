using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PresentationZoneTutorial : MonoBehaviour
{
    //Camera of the presentation
    public Camera cameraObject;

    SC_FPSController playerInputs;


    /// <summary>
    /// Detects if it's in collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        playerInputs = GameObject.FindGameObjectWithTag("Player").GetComponent<SC_FPSController>();
        playerInputs.setPresentationCamera(cameraObject);
    }

    /// <summary>
    /// Detects if it exits collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        playerInputs = GameObject.FindGameObjectWithTag("Player").GetComponent<SC_FPSController>();
        playerInputs.setPresentationCamera(null);
    }
}
