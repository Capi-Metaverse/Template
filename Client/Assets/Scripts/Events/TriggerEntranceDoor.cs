using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEntranceDoor : MonoBehaviour
{
    [SerializeField] private Animator EntranceDoor = null;
    private bool isOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isOpen == false)
        {
            EntranceDoor.Play("OfficeEntranceGlassDoor",0,0.0f);
            isOpen = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        EntranceDoor.Play("OfficeEntranceGlassDoorInverse",0,0.0f);
        isOpen = false;
    }
}
