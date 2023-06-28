using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private bool _isPaused;

    public bool IsPaused { get =>  _isPaused; set => _isPaused = value;}

    //InputHandler

    [SerializeField] private CharacterInputHandler _characterInputHandler;

    //Pause GameObject

    [SerializeField] private GameObject _pauseObject;

    //UI Manager

    //Static function to get the singleton
    public static PauseManager FindInstance()
    {
        return FindObjectOfType<PauseManager>();
    }



    public void Pause()
    {
        IsPaused = true;

        //_characterInputHandler.active = false;

        //UI Deactivate

        //Activate Pause Menu
        _pauseObject.SetActive(true);

    }

    public void Unpause()
    {
        IsPaused = false;

        //Deactivate Pause Menu
        _pauseObject.SetActive(false);

        //Activate UI

        //_characterInputHandler.active = true;

    }
}
