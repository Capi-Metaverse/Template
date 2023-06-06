using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using TMPro;
using System;
using System.Linq;



public class LoginManager : MonoBehaviour
{
    [Header("UI")]
    //UI Text
    public TMP_Text MessageText;

    //UI Inputs
    public TMP_InputField UsernameInput;
    public TMP_InputField EmailInput;
    public TMP_InputField PasswordInput;

    //UI Panels
    public GameObject LoginPanel;
    public GameObject RegisterPanel;
    public GameObject ResetPanel;

    //GameManager
    private GameManager GameManager;

    //Boolean that checks if the user is new or not
    private bool newUser = true;

    //Roles
   
    string UserRolePlayFab;

    //Player data Classes
    [Serializable]
    public class PlayerDataUsername
    {
        public string getPlayerUsername;
    }
    public class PlayerDataId
    {
        public string getPlayerId;
    }

    private int requestsCounter = 0;


    private void Start()
    {
        //It gets the GameManager at the start
        GameManager = GameManager.FindInstance();

        //If there's an error in the connection it will return
        //This statement indicates to the user that an error has occurred.
        if (GameManager.GetConnectionStatus() == ConnectionStatus.Failed)
        {
            MessageText.text = "An error has occurred. Try Again";
        }
    }





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
            MessageText.color = Color.red;
            MessageText.text = "Password too Short!";
            return;
        }
        if (TryGetComponent(out LoginPlayFab loginPlayFab))
        {
            loginPlayFab.Register( UsernameInput.text,EmailInput.text,PasswordInput.text);
        }//else(LoginSinPlayFab)
    }

    /*Reset Password Functions*/


    /// <summary>
    /// PlayFab. Sends a request to PlayFab to reset the password.
    /// </summary>
    public void ResetPassword()
    {
        if (TryGetComponent(out LoginPlayFab loginPlayFab))
        {
            loginPlayFab.OnReset(EmailInput.text);
        }//else(Log
    }


    /*Login Functions*/

    /// <summary>
    /// PlayFab. Sends a request to PlayFab with the login values (Email, Password).
    /// </summary>
    public void LoginButton()
    {
        MessageText.text = "logging in...";

        if (TryGetComponent(out LoginPlayFab loginPlayFab))
        {
            loginPlayFab.Login(EmailInput.text, PasswordInput.text);
            
        }//else(Log
    }
    

    /*Username Validation Message*/

    public bool ValidateUserName(string str)
    {
        // Check if string is null or empty
        if (string.IsNullOrEmpty(str))
        {
            NameErrorMessage("El nombre de usuario no puede estar vacío");
            return false;
        }
        // Check if string starts with a space
        if (str.StartsWith(" "))
        {
            NameErrorMessage("El nombre de usuario no puede empezar por un espacio en blanco");
            return false;
        }
        // Check if string is only spaces
        if (str.Trim().Length == 0)
        {
            NameErrorMessage("El nombre de usuario no debe estar contenido solo por espacios");
            return false;
        }
        // Check minimum length
        if (str.Length < 3)
        {
            NameErrorMessage("El nombre de usuario debe contener más de 3 caracteres");
            return false;
        }
        // Check maximum length
        if (str.Length > 20)
        {
            NameErrorMessage("El nombre de usuario debe contener menos de 20 caracteres");
            return false;
        }
        // Check forbidden characters
        string forbidden = "!@#$%^&*()+=";
        foreach (char c in forbidden)
        {
            if (str.Contains(c))
            {
                NameErrorMessage("El nombre de usuario contiene un carácter no permitido");
                return false;
            }
        }
        // Check reserved words
        string[] reserved = { "admin", "root", "system" };
        if (reserved.Contains(str.ToLower()))
        {
            NameErrorMessage("El nombre de usuario no debe contener palabras restringidas");
            return false;
        }
        // String is valid
        return true;
    }

    /// <summary>
    /// Change the style of the error message
    /// </summary>
    /// <param name="message"></param>
    public void NameErrorMessage(string message)
    {
        MessageText.fontSize = 4;
        MessageText.color = Color.red;
        MessageText.text = message;
    }

    public void SetInfoMessage(string message)
    {
        MessageText.text = message;
    }
}