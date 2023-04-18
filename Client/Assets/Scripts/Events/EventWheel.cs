using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventWheel : NetworkTransform
{
    public Animator animator;

    string animationToPlay = "";
    
    public void setAnimation1()
    {
        animationToPlay = "Clapping";
        Debug.Log(animationToPlay);
    }

    public void setAnimation2()
    {
        animationToPlay = "Waving";
        Debug.Log(animationToPlay);
    }

    public void setAnimation3()
    {
        animationToPlay = "Capoeira";
        Debug.Log(animationToPlay);
    }

    public override void Render()
    {
        if (animationToPlay != "") { 
            if (animator == null) { animator = this.gameObject.transform.parent.GetComponentInChildren<Animator>(); };
            animator.SetTrigger(animationToPlay);
            animationToPlay = "";
        }
    }
}
