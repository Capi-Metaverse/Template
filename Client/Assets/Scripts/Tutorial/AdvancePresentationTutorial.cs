using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancePresentationTutorial : MonoBehaviour, IMetaEvent
{

    [SerializeField] Presentation presentation;
    [SerializeField] TriggerDetector triggerDetector;

    [SerializeField] GameManagerTutorial gameManager;
    public void activate(bool host)
    {

        if (gameManager.TutorialStatus == TutorialStatus.Presentation)
        {
            presentation.OnAdvance();

            triggerDetector.OnRightArrow();
        }
    }
}
