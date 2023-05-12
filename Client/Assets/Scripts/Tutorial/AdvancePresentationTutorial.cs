using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancePresentationTutorial : MonoBehaviour, IMetaEvent
{

    [SerializeField] Presentation presentation;
    [SerializeField] TriggerDetector triggerDetector;

    GameObject _eventObject;
    GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }

    public void activate(bool host)
    {

      
            presentation.OnAdvance();

            triggerDetector.OnRightArrow();
       
    }
}
