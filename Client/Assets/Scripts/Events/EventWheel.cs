using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationList
{
    None,
    Clapping,
    Waving,
    Capoeira,
    Salute,
    Defeated,
    TwistedDance
}
public class EventWheel : NetworkTransform
{
    private Animator animator;

    private AnimationList previousAnimation;
    private AnimationList animationToPlay = AnimationList.None;
    
    public void setAnimation1()
    {
        animationToPlay = AnimationList.Clapping;
    }

    public void setAnimation2()
    {
        animationToPlay = AnimationList.Waving;
    }

    public void setAnimation3()
    {
        animationToPlay = AnimationList.Capoeira;
    }

    public void setAnimation4()
    {
        animationToPlay = AnimationList.Salute;
    }

    public void setAnimation5()
    {
        animationToPlay = AnimationList.Defeated;
    }

    public void setAnimation6()
    {
        animationToPlay = AnimationList.TwistedDance;
    }


    public override void Render()
    {
        if (animationToPlay != AnimationList.None)
        {
            if (animator == null) { animator = this.gameObject.transform.parent.GetComponentInChildren<Animator>(); }

            if (previousAnimation != animationToPlay)
            {
                if (previousAnimation != AnimationList.None) StartCoroutine(RunNewAnimation());
                else animator.SetInteger("AnimationWheel", (int)animationToPlay);
            }
            previousAnimation = animationToPlay;

            //Set the value to zero to end animation
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
            {
                Debug.Log("None animation");
                animationToPlay = AnimationList.None;
                previousAnimation = animationToPlay;
                animator.SetInteger("AnimationWheel", (int)animationToPlay);
            }
        }
    }

    IEnumerator RunNewAnimation()
    {
        Debug.Log("Apply animation");
        animator.SetInteger("AnimationWheel", (int)AnimationList.None);
        yield return new WaitForSeconds(0.1F);
        animator.SetInteger("AnimationWheel", (int)animationToPlay);
        Debug.Log(animationToPlay);

    }
}
