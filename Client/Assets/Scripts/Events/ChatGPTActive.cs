using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ChatGPTActive : MonoBehaviour, IMetaEvent
{
    public GameObject CanvasChatGPT;

    public void activate(bool host)
    {
        if (host)
        {
            CanvasChatGPT.SetActive(true);
           
        }
    }
}
