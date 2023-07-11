using UnityEngine;
using Fusion;


namespace PresentationModule
{
    /// <summary>
    /// Event to activate the slice back in the presentation
    /// </summary>
    public class BackPresentation : NetworkBehaviour, IMetaEvent
    {
        public Presentation presentation;
        GameObject _eventObject;
        PhotonManager photonManager;
        GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }

        public void activate(bool host)
        {
            photonManager = PhotonManager.FindInstance();
            RPCManager.RPC_BackPress(photonManager.Runner);
        }
    }
}