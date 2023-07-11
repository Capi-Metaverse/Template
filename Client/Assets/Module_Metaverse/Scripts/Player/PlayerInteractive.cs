using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UserCard;
namespace Player
{

    public class PlayerInteractive : MonoBehaviour, IMetaEvent
    {
        public PlayerInteractionE menuUiOther;
        GameObject _eventObject;
        GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }
        /// <summary>
        /// Script containing each avatar that allows them to be interactive, and to get the data from playfab
        /// </summary>
        /// <param name="host"></param>
        public void activate(bool host)
        {
            menuUiOther = GameObject.Find("Menus").GetComponent<PlayerInteractionE>();
            string playfabid = _eventObject.GetComponent<NetworkPlayer>().playfabIdentity;
            Debug.Log("PlayfabID del pulsado: " + playfabid);
            //get UIcard
            menuUiOther.GetPublicDataFromOtherPlayer(playfabid, new List<string> { "userUICard", "CustomImage", "Achievements" });

        }
    }
}
