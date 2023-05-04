using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEventTutorial : MonoBehaviour, IMetaEvent
{

    private bool active = false;
    public void activate(bool host)
    {
        active = !active;
        this.gameObject.GetComponentInChildren<Light>().enabled = active;


    }

    
}
