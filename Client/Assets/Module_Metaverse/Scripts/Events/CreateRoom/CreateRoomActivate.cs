using UnityEngine;

namespace CreateRoom
{
    public class CreateRoomActivate : MonoBehaviour, IMetaEvent
    {
        [SerializeField] private CreateRoomPanelUI createScript;
        GameObject _eventObject;
        GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }

        public void activate(bool host)
        {
            //Open UI
            createScript.OpenUI();
        }
    }
}
