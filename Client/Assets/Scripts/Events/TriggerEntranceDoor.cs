using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEntranceDoor : NetworkBehaviour
{
    [SerializeField] private Animator EntranceDoor = null;

    [Networked(OnChanged = nameof(OnChangedMemberInside))]
    public bool MemberInside { get; set; } = false;

    float startTime = 0.0f;

    private void OnTriggerStay(Collider other)
    {
        
        MemberInside = true;
        startTime = 5;

        
        while (startTime>0)
        {
            startTime -= Time.deltaTime;
        }
        MemberInside = false;

    }

    private void OnChangedMemberInside()
    {
        if (MemberInside)
        {
            EntranceDoor.Play("OfficeEntranceGlassDoor", 0, 0.0f);
        }
        else
        {
            EntranceDoor.Play("OfficeEntranceGlassDoorInverse", 0, 0.0f);
        }
    }

}