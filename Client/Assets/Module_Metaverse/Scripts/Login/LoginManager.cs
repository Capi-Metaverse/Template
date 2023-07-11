using UnityEngine;
using TMPro;
using System;
using Manager;
/// <summary>
/// Class. Check if the connection is stablished
/// </summary>
public class LoginManager : MonoBehaviour
{
    [Header("UI")]
    //UI Text
    public TMP_Text MessageText;

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
        PhotonManager photonManager = PhotonManager.FindInstance();

        //If there's an error in the connection it will return
        //This statement indicates to the user that an error has occurred.
        if (photonManager.ConnectionStatus == ConnectionStatus.Failed)
        {
            MessageText.text = "An error has occurred. Try Again";
        }
    }
}