using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionArea : MonoBehaviour
{
    private SC_FPSController playerController;
    public UnityEvent detectionEvent;
    private void Update()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<SC_FPSController>();
    }
    //Detect if it in collider
    private void OnTriggerEnter(Collider other) {
        Debug.Log("Contacto");
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("La colision fue con el Player");
            detectionEvent.Invoke();
        }
    }
    //Detect if Exits collider
    private void OnTriggerExit(Collider other) {
        Debug.Log("Salida");
        playerController.enabled=true;
    }
    //Change the location
    public void Respawn(Transform pointToSpawn)
    {
        playerController.enabled=false;
        Debug.Log(playerController.transform.position);
        playerController.transform.position = pointToSpawn.position;
    }
}
