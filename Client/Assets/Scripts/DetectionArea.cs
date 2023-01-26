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
    private void OnTriggerEnter(Collider other) {
        Debug.Log("Contacto");
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("La colision fue con el Player");
            detectionEvent.Invoke();
        }
    }
     private void OnTriggerExit(Collider other) {
        Debug.Log("Salida");
        playerController.enabled=true;
    }
    public void Respawn(Transform pointToSpawn)
    {
        playerController.enabled=false;
        Debug.Log(playerController.transform.position);
        playerController.transform.position = pointToSpawn.position;
    }
}
