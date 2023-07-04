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
    public NetworkBool IsStopped { get; set; } = false;


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


        if (IsStopped && IsPlaying)
        {
           
            animator.SetInteger("AnimationWheel", (int)AnimationList.None);
            if (this.gameObject.transform.parent.GetComponent<NetworkPlayer>().ActorID == PhotonManager.FindInstance().Runner.LocalPlayer)
            {
                IsStopped = false;
                animationToPlay = AnimationList.None;

            }
            IsPlaying = false;



        }


        if (animationToPlay != AnimationList.None && !IsPlaying && !IsStopped)
        {
            IsPlaying = true;
            animator.SetInteger("AnimationWheel", (int)animationToPlay);

        }

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0) && !IsStopped)
        {
          
            animator.SetInteger("AnimationWheel", (int)AnimationList.None);
            if (this.gameObject.transform.parent.GetComponent<NetworkPlayer>().ActorID == PhotonManager.FindInstance().Runner.LocalPlayer)
            {
                animationToPlay = AnimationList.None;
            }
            IsPlaying = false;
        }



    }

    /// <summary>
    /// Controls which animation is being played.
    /// </summary>
    public override void Render()
    {
      

       
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
