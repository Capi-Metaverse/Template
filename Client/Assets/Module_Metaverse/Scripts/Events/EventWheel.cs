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

    [SerializeField]
    public AnimationList previousAnimation { get; set; } = AnimationList.None;

    [Networked]
    [SerializeField]
    public AnimationList animationToPlay { get; set; } = AnimationList.None;

    [SerializeField]
    private bool IsPlaying = false;

    [Networked]
    public NetworkBool IsStopped { get; set; }


    private void Start()
    {
        if (animator == null) { animator = this.gameObject.transform.parent.GetComponentInChildren<Animator>(); }
    }

    public void setAnimation1()
    {
        ResetAnimation();
        animationToPlay = AnimationList.Clapping;
    }

    public void setAnimation2()
    {
        ResetAnimation();
        animationToPlay = AnimationList.Waving;
    }

    public void setAnimation3()
    {
        ResetAnimation();
        animationToPlay = AnimationList.Capoeira;
    }

    public void setAnimation4()
    {
        ResetAnimation();
        animationToPlay = AnimationList.Salute;
    }

    public void setAnimation5()
    {
        ResetAnimation();
        animationToPlay = AnimationList.Defeated;
    }

    public void setAnimation6()
    {
        ResetAnimation();
        animationToPlay = AnimationList.TwistedDance;
    }

    public override void FixedUpdateNetwork()
    {


        if (IsStopped)
        {
            AnimationList newAnimation = animationToPlay;
           
            animator.SetInteger("AnimationWheel", (int)AnimationList.None);
            if (this.gameObject.transform.parent.GetComponent<NetworkPlayer>().ActorID == PhotonManager.FindInstance().Runner.LocalPlayer)
            {
                IsStopped = false;

            }
            animationToPlay = newAnimation;

            animator.SetInteger("AnimationWheel", (int)animationToPlay);
            IsPlaying = true;



        }


        if (animationToPlay != AnimationList.None && !IsPlaying && !IsStopped)
        {
            IsPlaying = true;
            animator.SetInteger("AnimationWheel", (int)animationToPlay);

        }

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0) && !IsStopped)
        {
            IsPlaying = false;
            animator.SetInteger("AnimationWheel", (int)AnimationList.None);
            if (this.gameObject.transform.parent.GetComponent<NetworkPlayer>().ActorID == PhotonManager.FindInstance().Runner.LocalPlayer)
            {
                animationToPlay = AnimationList.None;
            }
        }



    }

    /// <summary>
    /// Controls which animation is being played.
    /// </summary>
    public override void Render()
    {
      

        /*
        //Animation
        if (animationToPlay != AnimationList.None)
        {

            //Animation Changed
            if (previousAnimation != animationToPlay)
            {
              
                //Animation Stopped
                if (previousAnimation != AnimationList.None)
                {
                  
                    StartCoroutine(RunNewAnimation());
                }
                //First Time
                else
                {
                    animator.SetInteger("AnimationWheel", (int)animationToPlay); 
                }
            }
            previousAnimation = animationToPlay;

            //Set the value to zero to end animation
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
            {

            animationToPlay = AnimationList.None;
            previousAnimation = animationToPlay;
          
                    
            animator.SetInteger("AnimationWheel", (int)animationToPlay);
            }
        }
        */
    }

    private void ResetAnimation()
    {
        if (IsPlaying)
        {
            IsStopped = true;
            Debug.Log("Stopping");
        }
    }
    /// <summary>
    /// Starts running a new animation.
    /// </summary>
    /// <returns></returns>
    IEnumerator RunNewAnimation()
    { 
        yield return new WaitForSeconds(0.1F);
        animator.SetInteger("AnimationWheel", (int)animationToPlay);
  

    }
}
