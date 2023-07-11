using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Event
{
    public class PrimarySpeaker : MonoBehaviour, IMetaEvent
    {
        GameObject _eventObject;
        GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }
        PhotonManager photonManager;
        /// <summary>
        /// detects audio from other players, using GameManager
        /// </summary>
        /// <param name="host"></param>
        public void activate(bool host)
        {
            photonManager = PhotonManager.FindInstance();
            RPCManager.RPC_PrimarySpeaker(photonManager.Runner, photonManager.Runner.LocalPlayer);
        }
    }
}

