using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimarySpeaker : MonoBehaviour, IMetaEvent
{
    GameObject _eventObject;
    GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }

    public void activate(bool host)
    {
        GameManager gameManager = GameManager.FindInstance();
        GameManager.RPC_PrimarySpeaker(gameManager.GetRunner(), gameManager.GetRunner().LocalPlayer);
    }
}
