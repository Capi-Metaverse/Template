using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private bool _isPaused = false;

    public bool IsPaused { get =>  _isPaused; set => _isPaused = value;}

    //InputHandler
    [SerializeField] private CharacterInputHandler _characterInputHandler;


    //UI Manager

    //Static function to get the singleton
    public static PauseManager FindInstance()
    {
        return FindObjectOfType<PauseManager>();
    }


    /// <summary>
    /// Static function to get the singleton
    /// </summary>
    /// <returns></returns>
    public void Pause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        IsPaused = true;


    }

    /// <summary>
    /// Set the game state to play
    /// </summary>
    public void Unpause()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        IsPaused = false;

    }
}
