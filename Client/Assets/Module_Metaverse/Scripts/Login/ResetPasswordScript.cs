using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResetPasswordScript : MonoBehaviour
{
    //Inputs
    public TMP_InputField EmailInput;

    //UI Manager
    public PanelLoginManager PanelLoginManager;

    //Login Manager
    private GameObject LoginManager;

    private void Start()
    {
        LoginManager = GameObject.Find("LoginManager");
    }

    /// <summary>
    /// PlayFab. Sends a request to PlayFab to reset the password.
    /// </summary>
    public void ResetPassword()
    {
        if (LoginManager.TryGetComponent(out LoginPlayFab loginPlayFab))
        {
            loginPlayFab.OnReset(EmailInput.text);
        }
        //TODO: Add ELSE which will do the reset without playfab
    }
}
