using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventWheelTutorial : MonoBehaviour
{
    [SerializeField] private GameManagerTutorial gameManager;

    public void setAnimation1()
    {
        gameManager.AnimationToPlay = AnimationList.Clapping;
    }

    public void setAnimation2()
    {
        gameManager.AnimationToPlay = AnimationList.Waving;
    }

    public void setAnimation3()
    {
        gameManager.AnimationToPlay = AnimationList.Capoeira;
    }

    public void setAnimation4()
    { 
        gameManager.AnimationToPlay = AnimationList.Salute;
    }

    public void setAnimation5()
    {
        gameManager.AnimationToPlay = AnimationList.Defeated;
    }

    public void setAnimation6()
    {
        gameManager.AnimationToPlay = AnimationList.TwistedDance;
    }
}
