using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PresentationModule;

namespace Tutorial
{
    public class BackPresentationTutorial : MonoBehaviour, IMetaEvent
    {
        [SerializeField] Presentation presentation;

        [SerializeField] TriggerDetector triggerDetector;

        GameObject _eventObject;
        GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }

        /// <summary>
        /// Activates the OnLeftArrow function.
        /// </summary>
        /// <param name="host"></param>
        public void activate(bool host)
        {
            presentation.OnReturn();

            triggerDetector.OnLeftArrow();
        }
    }
}
