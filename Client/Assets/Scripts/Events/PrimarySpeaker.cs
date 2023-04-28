using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimarySpeaker : MonoBehaviour, IMetaEvent
{
    public void activate(bool host)
    {
        GameManager gameManager = GameManager.FindInstance();
        GameManager.RPC_PrimarySpeaker(gameManager.GetRunner(), gameManager.GetRunner().LocalPlayer);
    }
}
