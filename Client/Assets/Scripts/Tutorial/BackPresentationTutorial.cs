using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackPresentationTutorial : MonoBehaviour, IMetaEvent
{
    [SerializeField] Presentation presentation;

    [SerializeField] TriggerDetector triggerDetector;

    [SerializeField] GameManagerTutorial gameManager;

    public void activate(bool host)
    {


        if (gameManager.TutorialStatus == TutorialStatus.Presentation)
        {
            presentation.OnReturn();

            triggerDetector.OnLeftArrow();
        }
    }
}
