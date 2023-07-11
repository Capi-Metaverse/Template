using UnityEngine;
using Animation;

namespace Tutorial
{
    /// <summary>
    /// Controls which animations are played when interacting with the Emote Wheel.
    /// </summary>
    public class EventWheelTutorial : MonoBehaviour
    {
        [SerializeField] private GameManagerTutorial gameManager;

        /// <summary>
        /// Sets the first animation.
        /// </summary>
        public void setAnimation1()
        {
            gameManager.AnimationToPlay = AnimationList.Clapping;
        }

        /// <summary>
        /// Sets the second animation.
        /// </summary>
        public void setAnimation2()
        {
            gameManager.AnimationToPlay = AnimationList.Waving;
        }

        /// <summary>
        /// Sets the third animation.
        /// </summary>
        public void setAnimation3()
        {
            gameManager.AnimationToPlay = AnimationList.Capoeira;
        }

        /// <summary>
        /// Sets the fourth animation.
        /// </summary>
        public void setAnimation4()
        {
            gameManager.AnimationToPlay = AnimationList.Salute;
        }

        /// <summary>
        /// Sets the fifth animation.
        /// </summary>
        public void setAnimation5()
        {
            gameManager.AnimationToPlay = AnimationList.Defeated;
        }

        /// <summary>
        /// Sets the sixth animation.
        /// </summary>
        public void setAnimation6()
        {
            gameManager.AnimationToPlay = AnimationList.TwistedDance;
        }
    }
}

