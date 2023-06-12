using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Class. Change the UI of the login from current to selected, example:
/// (LoginPanel-->RegisterPanel, RegisterPanel-->ResetPasswordPanel)
/// </summary>
public class PanelManager : MonoBehaviour
{
    //Info UI Text
    public TMP_Text MessageText;

    //UI Panels
    public GameObject LoginPanel;
    public GameObject RegisterPanel;
    public GameObject ResetPanel;

    //-------------------- Panel Modification Functions --------------------

    /// <summary>
    /// This function change the current UI to the Sign-Up UI
    /// </summary>
    public void ChangeRegister()
    {
        RegisterPanel.SetActive(true);
        ResetPanel.SetActive(false);
        LoginPanel.SetActive(false);
    }

    /// <summary>
    /// This function change the current UI to the Login UI
    /// </summary>
    public void ChangeLogin()
    {
        RegisterPanel.SetActive(false);
        ResetPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }

    /// <summary>
    /// This Function change the current UI to the Reset Password UI
    /// </summary>
    public void ChangeReset()
    {
        RegisterPanel.SetActive(false);
        ResetPanel.SetActive(true);
        LoginPanel.SetActive(false);
    }

    /// <summary>
    /// Display a message to inform about something needed
    /// </summary>
    public void SetInfoMessage(string message)
    {
        MessageText.color = Color.white;
        MessageText.text = message;
    }

    /// <summary>
    /// Display a message to inform about an error
    /// </summary>
    public void SetErrorMessage(string message)
    {
        MessageText.color = Color.red;
        MessageText.text = message;
    }
}
