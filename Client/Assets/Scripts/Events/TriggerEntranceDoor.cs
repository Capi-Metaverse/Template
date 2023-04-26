using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEntranceDoor : MonoBehaviour
{
    [SerializeField] private Animator EntranceDoor = null;
    private int usersInTrigger = 0;

    private void OnTriggerEnter(Collider other)
    {
        usersInTrigger++;
        if (usersInTrigger == 1)
        {
            EntranceDoor.Play("OfficeEntranceGlassDoor", 0, 0.0f);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        usersInTrigger--;
        if (usersInTrigger == 0)
        {
            EntranceDoor.Play("OfficeEntranceGlassDoorInverse", 0, 0.0f);
        }
    }
}