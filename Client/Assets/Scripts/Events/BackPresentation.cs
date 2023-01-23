using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackPresentation : MonoBehaviour, IMetaEvent
{

   public Presentation presentation;

     public void activate(){

        presentation.OnReturn();

     }
}
