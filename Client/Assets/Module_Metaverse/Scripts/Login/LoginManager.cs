using UnityEngine;
using TMPro;
using System;

/// <summary>
/// Class. Check if the connection is stablished
/// </summary>
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