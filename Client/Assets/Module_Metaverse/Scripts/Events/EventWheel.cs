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
    [SerializeField]
    public AnimationList animationToPlay { get; set; } = AnimationList.None;

    public bool IsPlaying { get; set; } = false;

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

    public override void FixedUpdateNetwork()
    {

        if (animationToPlay != AnimationList.None )
        {
            if(!IsPlaying) IsPlaying = true;


            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
            {
                IsPlaying = false;

             if (this.gameObject.transform.parent.GetComponent<NetworkPlayer>().ActorID == PhotonManager.FindInstance().Runner.LocalPlayer)
                {

                    animationToPlay = AnimationList.None;

                }

            }

        }
    
    }

    /// <summary>
    /// Controls which animation is being played.
    /// </summary>
    public override void Render()
    {
        animator.SetInteger("AnimationWheel", (int)animationToPlay);


    }
  
}
