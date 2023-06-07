using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelManager : MonoBehaviour
{

    //Info UI Text
    public TMP_Text MessageText;

    //UI Panels
    public GameObject LoginPanel;
    public GameObject RegisterPanel;
    public GameObject ResetPanel;




    //-------------------- Panel Modification Functions --------------------


    //Function that changes the UI to the Sign-Up UI
    public void ChangeRegister()
    {
        RegisterPanel.SetActive(true);
        ResetPanel.SetActive(false);
        LoginPanel.SetActive(false);
    }
    //Function that changes the UI to the Login UI
    public void ChangeLogin()
    {
        RegisterPanel.SetActive(false);
        ResetPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }

    //Function that changes the UI to the Reset Password UI
    public void ChangeReset()
    {
        RegisterPanel.SetActive(false);
        ResetPanel.SetActive(true);
        LoginPanel.SetActive(false);
    }

    public void SetInfoMessage(string message)
    {
        MessageText.color = Color.white;
        MessageText.text = message;
    }

    public void SetErrorMessage(string message)
    {
        MessageText.color = Color.red;
        MessageText.text = message;
    }



}
