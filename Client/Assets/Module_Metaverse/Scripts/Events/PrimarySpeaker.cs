using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimarySpeaker : MonoBehaviour, IMetaEvent
{
    GameObject _eventObject;
    GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }
    /// <summary>
    /// detects audio from other players, using GameManager
    /// </summary>
    /// <param name="host"></param>
    public void activate(bool host)
    {
        GameManager gameManager = GameManager.FindInstance();
        RPCManager.RPC_PrimarySpeaker(gameManager.GetRunner(), gameManager.GetRunner().LocalPlayer);
    }
}
