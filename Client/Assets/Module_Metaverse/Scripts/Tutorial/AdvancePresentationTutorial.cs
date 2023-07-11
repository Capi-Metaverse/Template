using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PresentationModule;

namespace Tutorial
{
    public class AdvancePresentationTutorial : MonoBehaviour, IMetaEvent
    {

        [SerializeField] Presentation presentation;
        [SerializeField] TriggerDetector triggerDetector;

        GameObject _eventObject;
        GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }

        /// <summary>
        /// Activates the OnRightArrow function.
        /// </summary>
        /// <param name="host"></param>
        public void activate(bool host)
        {
            presentation.OnAdvance();

            triggerDetector.OnRightArrow();
        }
    }
}
