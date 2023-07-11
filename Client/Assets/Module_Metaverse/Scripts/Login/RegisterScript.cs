using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace LoginModule
{
    public class RegisterScript : MonoBehaviour
    {

        //Inputs
        public TMP_InputField EmailInput;
        public TMP_InputField UsernameInput;
        public TMP_InputField PasswordInput;

        //UI Manager
        private PanelLoginManager PanelLoginManager;

        //Login Manager
        private GameObject LoginManager;

        private void Start()
        {
            LoginManager = GameObject.Find("LoginManager");
            PanelLoginManager = GameObject.Find("PanelLoginManager").GetComponent<PanelLoginManager>();
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
                PanelLoginManager.SetErrorMessage("Password too short");
                return;
            }

            if (LoginManager.TryGetComponent(out LoginPlayFab loginPlayFab))
            {
                loginPlayFab.Register(UsernameInput.text, EmailInput.text, PasswordInput.text);
            }
            //TODO: Add ELSE which will do the register without playfab
        }

        /*Username Validation Message*/

        public bool ValidateUserName(string str)
        {
            // Check if string is null or empty
            if (string.IsNullOrEmpty(str))
            {
                PanelLoginManager.SetErrorMessage("El nombre de usuario no puede estar vacío");
                return false;
            }
            // Check if string starts with a space
            if (str.StartsWith(" "))
            {
                PanelLoginManager.SetErrorMessage("El nombre de usuario no puede empezar por un espacio en blanco");
                return false;
            }
            // Check if string is only spaces
            if (str.Trim().Length == 0)
            {
                PanelLoginManager.SetErrorMessage("El nombre de usuario no debe estar contenido solo por espacios");
                return false;
            }
            // Check minimum length
            if (str.Length < 3)
            {
                PanelLoginManager.SetErrorMessage("El nombre de usuario debe contener más de 3 caracteres");
                return false;
            }
            // Check maximum length
            if (str.Length > 20)
            {
                PanelLoginManager.SetErrorMessage("El nombre de usuario debe contener menos de 20 caracteres");
                return false;
            }
            // Check forbidden characters
            string forbidden = "!@#$%^&*()+=";
            foreach (char c in forbidden)
            {
                if (str.Contains(c))
                {
                    PanelLoginManager.SetErrorMessage("El nombre de usuario contiene un carácter no permitido");
                    return false;
                }
            }
            // Check reserved words
            string[] reserved = { "admin", "root", "system" };
            if (reserved.Contains(str.ToLower()))
            {
                PanelLoginManager.SetErrorMessage("El nombre de usuario no debe contener palabras restringidas");
                return false;
            }
            // String is valid
            return true;
        }

    }

}
