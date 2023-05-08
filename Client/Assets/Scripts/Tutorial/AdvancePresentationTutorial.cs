using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancePresentationTutorial : MonoBehaviour, IMetaEvent
{

    [SerializeField] Presentation presentation;
    [SerializeField] TriggerDetector triggerDetector;
    public void activate(bool host)
    {
        presentation.OnAdvance();

        triggerDetector.OnRightArrow();
    }
}
