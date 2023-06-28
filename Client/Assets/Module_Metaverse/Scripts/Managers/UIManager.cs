using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
  
     [SerializeField] private PlayerUI _playerUI;

     public PlayerUI PlayerUI { get => _playerUI; set => _playerUI = value; }

    //Static function to get the singleton
    public static UIManager FindInstance()
    {
        return FindObjectOfType<UIManager>();
    }

    public void SetUIOn()
    {

        Debug.Log("Activate UI");
        PlayerUI.ShowUI();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
       
       
    }

    public void SetUIOff()
    {
        Debug.Log("Deactivate UI");
        PlayerUI.HideUI();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        

    }

    public void HideEventText()
    {
        PlayerUI.EventTextOff();
        
    }

    public void ShowEventText()
    {
        PlayerUI.EventTextOn();
      
    }

    public void HidePresentationText()
    {
        PlayerUI.PresentationTextOff();

    }

    public void ShowPresentationText()
    {
        PlayerUI.PresentationTextOn();

    }


}
