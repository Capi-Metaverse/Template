using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancePresentation : MonoBehaviour, IMetaEvent
{
    public Presentation presentation;

     public void activate(){

        presentation.OnAdvance();

     }
    
}
