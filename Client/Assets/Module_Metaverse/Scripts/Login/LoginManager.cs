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





 
}