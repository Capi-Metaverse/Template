using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RegisterScript : MonoBehaviour
{

    //Inputs
    public TMP_InputField EmailInput;
    public TMP_InputField UsernameInput;
    public TMP_InputField PasswordInput;

    //UI Manager
    private PanelManager PanelManager;

    //Login Manager
    private GameObject LoginManager;

    private void Start()
    {
        LoginManager = GameObject.Find("LoginManager");
        PanelManager = GameObject.Find("PanelManager").GetComponent<PanelManager>();
    }



    /*Register Functions*/

    /// <summary>
    /// PlayFab. Confirms that the password reaches the length required and 
    /// sends a request to PlayFab to register the new user.
    /// </summary>
    public void RegisterButton()
    {
        //llamada a la interfaz
        if (!ValidateUserName(UsernameInput.text))
        {
            return;
        }
        if (PasswordInput.text.Length < 6)
        {
            PanelManager.SetErrorMessage("Password too short");
            return;
        }

        if (LoginManager.TryGetComponent(out LoginPlayFab loginPlayFab))
        {
            loginPlayFab.Register(UsernameInput.text, EmailInput.text, PasswordInput.text);
        }//else(LoginSinPlayFab)
    }

    /*Username Validation Message*/

    public bool ValidateUserName(string str)
    {
        // Check if string is null or empty
        if (string.IsNullOrEmpty(str))
        {
            PanelManager.SetErrorMessage("El nombre de usuario no puede estar vac�o");
            return false;
        }
        // Check if string starts with a space
        if (str.StartsWith(" "))
        {
            PanelManager.SetErrorMessage("El nombre de usuario no puede empezar por un espacio en blanco");
            return false;
        }
        // Check if string is only spaces
        if (str.Trim().Length == 0)
        {
            PanelManager.SetErrorMessage("El nombre de usuario no debe estar contenido solo por espacios");
            return false;
        }
        // Check minimum length
        if (str.Length < 3)
        {
            PanelManager.SetErrorMessage("El nombre de usuario debe contener m�s de 3 caracteres");
            return false;
        }
        // Check maximum length
        if (str.Length > 20)
        {
            PanelManager.SetErrorMessage("El nombre de usuario debe contener menos de 20 caracteres");
            return false;
        }
        // Check forbidden characters
        string forbidden = "!@#$%^&*()+=";
        foreach (char c in forbidden)
        {
            if (str.Contains(c))
            {
                PanelManager.SetErrorMessage("El nombre de usuario contiene un car�cter no permitido");
                return false;
            }
        }
        // Check reserved words
        string[] reserved = { "admin", "root", "system" };
        if (reserved.Contains(str.ToLower()))
        {
            PanelManager.SetErrorMessage("El nombre de usuario no debe contener palabras restringidas");
            return false;
        }
        // String is valid
        return true;
    }

}
