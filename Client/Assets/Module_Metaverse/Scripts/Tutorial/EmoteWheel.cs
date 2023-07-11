using UnityEngine;
using Animation;

namespace Tutorial
{
    public class EmoteWheel : MonoBehaviour
    {

        [SerializeField] private GameManagerTutorial gameManager;

        /// <summary>
        /// Set the animation variable for the EventWheel
        /// </summary>
        /// <param name="animation"></param>
        public void SetAnimation(int animation)
        {
            gameManager.AnimationToPlay = (AnimationList)animation;

        }
    }
}

