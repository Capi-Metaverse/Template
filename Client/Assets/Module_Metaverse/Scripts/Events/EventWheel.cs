using Fusion;
using UnityEngine;

namespace Animation
{
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

        /// <summary>
        /// Set the animation variable for the EventWheel
        /// </summary>
        /// <param name="animation"></param>
        public void SetAnimation(int animation)
        {
            animationToPlay = (AnimationList)animation;

        }
        /// <summary>
        /// Runs before the Render
        /// </summary>
        public override void FixedUpdateNetwork()
        {

            if (animationToPlay != AnimationList.None)
            {
                if (!IsPlaying) IsPlaying = true;

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

}
