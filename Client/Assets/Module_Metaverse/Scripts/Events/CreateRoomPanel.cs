using UnityEngine;

public class CreateRoomPanel : MonoBehaviour, IMetaEvent
{
    [SerializeField] private CreateRoom createScript;
    GameObject _eventObject;
    GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }

    public void activate(bool host)
    {
        //Open UI
        createScript.OpenUI();
    }
}
