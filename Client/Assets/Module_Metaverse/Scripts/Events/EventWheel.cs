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
    public Animator animator;

    [Networked]
    public AnimationList previousAnimation { get; set; } = AnimationList.None;

    [Networked]
    public AnimationList animationToPlay { get; set; } = AnimationList.None;

  

    private void Start()
    {
        if (animator == null) { animator = this.gameObject.transform.parent.GetComponentInChildren<Animator>(); }
    }

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

    /// <summary>
    /// Controls which animation is being played.
    /// </summary>
    public override void Render()
    {
        //Animation
        if (animationToPlay != AnimationList.None)
        {
            Debug.Log(animationToPlay);
            Debug.Log(previousAnimation);

            //Animation Changed
            if (previousAnimation != animationToPlay)
            {
                Debug.Log("prueba");
                //Animation Stopped
                if (previousAnimation != AnimationList.None)
                {
                    Debug.Log("Segundo if");
                    StartCoroutine(RunNewAnimation());
                }
                //First Time
                else
                {
                    Debug.Log("Else");
                    Debug.Log(animationToPlay.ToString());
                    
                    animator.SetInteger("AnimationWheel", (int)animationToPlay); 
                }
            }
            if(PhotonManager.FindInstance().CurrentPlayer.GetComponent<NetworkPlayer>().ActorID == Runner.LocalPlayer)
            previousAnimation = animationToPlay;

            //Set the value to zero to end animation
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
            {
                Debug.Log("None animation");
                Debug.Log(PhotonManager.FindInstance().CurrentPlayer.GetComponent<NetworkPlayer>().ActorID);
                Debug.Log(Runner.LocalPlayer);

                if (PhotonManager.FindInstance().CurrentPlayer.GetComponent<NetworkPlayer>().ActorID == Runner.LocalPlayer)
                {
                    animationToPlay = AnimationList.None;
                    previousAnimation = animationToPlay;
                }
                    
                animator.SetInteger("AnimationWheel", (int)animationToPlay);
            }
        }
    }
    /// <summary>
    /// Starts running a new animation.
    /// </summary>
    /// <returns></returns>
    IEnumerator RunNewAnimation()
    {
        Debug.Log("Apply animation");
        animator.SetInteger("AnimationWheel", (int)AnimationList.None);
        yield return new WaitForSeconds(0.1F);
        animator.SetInteger("AnimationWheel", (int)animationToPlay);
        Debug.Log(animationToPlay);

    }
}
