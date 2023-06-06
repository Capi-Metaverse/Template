using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;


public class LoginManager : MonoBehaviour
{
    [Header("UI")]
    //UI Text
    public TMP_Text messageText;

    //UI Inputs
    public TMP_InputField usernameInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    //UI Panels
    public GameObject LoginPanel;
    public GameObject RegisterPanel;
    public GameObject ResetPanel;

    //GameManager
    private GameManager gameManager;

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
        gameManager = GameManager.FindInstance();

        //If there's an error in the connection it will return
        //This statement indicates to the user that an error has occurred.
        if (gameManager.GetConnectionStatus() == ConnectionStatus.Failed)
        {
            messageText.text = "An error has occurred. Try Again";
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
        if (!ValidateUserName(usernameInput.text))
        {
            return;
        }
        if (passwordInput.text.Length < 6)
        {
            messageText.color = Color.red;
            messageText.text = "Password too Short!";
            return;
        }
        if (TryGetComponent(out LoginPlayFab loginPlayFab))
        {
            loginPlayFab.Register( usernameInput.text,emailInput.text,passwordInput.text);
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
            loginPlayFab.Reset(emailInput.text);
        }//else(Log
    }


    /*Login Functions*/

    /// <summary>
    /// PlayFab. Sends a request to PlayFab with the login values (Email, Password).
    /// </summary>
    public void LoginButton()
    {
        messageText.text = "logging in...";

        if (TryGetComponent(out LoginPlayFab loginPlayFab))
        {
            loginPlayFab.Login(emailInput.text, passwordInput.text);
            
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
        messageText.fontSize = 4;
        messageText.color = Color.red;
        messageText.text = message;
    }
}