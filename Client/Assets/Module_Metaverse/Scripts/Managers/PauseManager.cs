using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private bool _isPaused = false;

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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        IsPaused = true;


    }

    public void Unpause()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        IsPaused = false;

    }
}
