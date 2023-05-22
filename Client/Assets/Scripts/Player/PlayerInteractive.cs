using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractive : MonoBehaviour, IMetaEvent
{
    public PlayerInteractionE menuUiOther;
    GameObject _eventObject;
    GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }
    //
    public void activate(bool host)
    {
        menuUiOther = GameObject.Find("Menus").GetComponent<PlayerInteractionE>();
        string playfabid = _eventObject.GetComponent<NetworkPlayer>().playfabIdentity;
        Debug.Log("PlayfabID del pulsado: " + playfabid);
        //get UIcard
        menuUiOther.GetPublicDataFromOtherPlayer(playfabid, new List<string> { "userUICard", "CustomImage" });

    }
}
