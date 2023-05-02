using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    //PlayerUIPrefab
    GameObject scope;
    GameObject micro;//Actually this is the microphone in game
    //public GameObject eventText;

    public SC_FPSController fpsController;
    public GameObject canvas;

    

    private void Start()
    {
        GameObject player = GameObject.Find("TonyModel");
        //micro = transform.GetChild(3).GetChild(0).gameObject;//Micro
        //scope = GameManager.FindInstance().GetCurrentPlayer().transform.GetChild(3).GetChild(1).gameObject;//Scope
        //eventText = GameManager.FindInstance().GetCurrentPlayer().transform.GetChild(3).GetChild(2).gameObject;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            CloseMenu();

        }
    }



    public void OpenMenu()
        {
        
        fpsController.enabled = false;
        canvas.SetActive(true);
        //eventText.SetActive(false);
        //micro.SetActive(false);
        //scope.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseMenu() {
        fpsController.enabled = true;
        canvas.SetActive(false);

        //eventText.SetActive(true);
        //micro.SetActive(true);
        //scope.SetActive(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        this.enabled = false;


    }
      

   
      


}
