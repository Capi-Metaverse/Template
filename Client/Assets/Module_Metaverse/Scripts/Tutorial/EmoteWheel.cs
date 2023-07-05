using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
