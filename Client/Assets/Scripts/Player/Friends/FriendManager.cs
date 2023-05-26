using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Unity.VisualScripting;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;


//Class that represents a friend of the current user in the application
public class Friend
{
    private string username;
    private string id;
    private string tags;

    public string Username { get => username; set => username = value; }
    public string Id { get => id; set => id = value; }
    public string Tags { get => tags; set => tags = value; }
}

public class FriendManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputFriendUsername; // The username of the friend to be added
    [SerializeField] private GameObject panelMessageCheckName; // The panel of the validation message
    [SerializeField] private TextMeshProUGUI validationMessage;// The validation message
    private FriendList friendList;

    private GameManager gameManager;
    private List<Friend> friends = new List<Friend>();
    private string userId = "";

    public List<Friend> Friends { get => friends; set => friends = value; }

    public void Start()
    {
        gameManager = GameManager.FindInstance();
        friendList = gameObject.GetComponent<FriendList>();
    }
    /// <summary>
    /// PlayFab - Method that gets the ID of a player from PlayFab
    /// </summary>
    public void RequestIDPlayer()
    {
        if (ValidarUserNameFriend(inputFriendUsername.text))
        {
            var result = new GetAccountInfoRequest { Username = inputFriendUsername.text };

            PlayFabClientAPI.GetAccountInfo(result, OnGetAccountInfoSuccess, OnGetAccountInfoFailure);
        }
    }
    //Callback from a successfull Request ID Player 
    private void OnGetAccountInfoSuccess(GetAccountInfoResult result)
    {
        userId = result.AccountInfo.PlayFabId;
        Debug.Log("User ID: " + userId);

        SendFriendRequest();
    }

    //Callback from a failed Request ID Player 
    private void OnGetAccountInfoFailure(PlayFabError error)
    {
        Debug.LogError("GetAccountInfo failed: " + error.ErrorMessage);
    }
    /// <summary>
    /// Method assigned to the Add Friend Button
    /// </summary>
    public void AddFriend()
    {
        if (ValidarUserNameFriend(inputFriendUsername.text))
        {
            RequestIDPlayer();
           
        }

        //Else Error Message
    }
    /// <summary>
    /// PlayFab - Method that sends the Friend Request on PlayFab
    /// </summary>
    public void SendFriendRequest()
    {
        Debug.Log("Send info");
            // Call the PlayFab Cloud Script function to add the friend
            ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
            {
                FunctionName = "SendFriendRequest", // Replace with the name of your Cloud Script function
                FunctionParameter = new
                {
                    friendUsername = inputFriendUsername.text,
                    friendplayfabid = userId
                }, // Pass in any required parameters
                GeneratePlayStreamEvent = true // Set to true if you want PlayStream events to be generated for this API call
            };

            PlayFabClientAPI.ExecuteCloudScript(request, OnAddFriendSuccess, OnAddFriendFailure);
        
        userId = "";
        inputFriendUsername.text = null; // Clean the friend name field

    }

    //Callback from a successfull SendFriendRequest
    private void OnAddFriendSuccess(ExecuteCloudScriptResult result)
    {
        Debug.Log(result.FunctionResult);
        // Handle success, if necessary
    }

    //Callback from a failed SendFriendRequest
    private void OnAddFriendFailure(PlayFabError error)
    {
        Debug.LogError("Failed to add friend: " + error.ErrorMessage);
        // Handle failure, if necessary
    }
    /// <summary>
    /// PlayFab - Method that returns the list of friends from Playfab
    /// </summary>
    public void GetFriendsConfirmedList()
    {
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
        {
            FunctionName = "getFriendsListRequester",
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnGetFriendsConfirmedListSuccess, OnGetFriendsListFailure);
    }

    /// <summary>
    /// Callback for successful GetFriendsList ,which gets the result of the callback and appends it to a list
    /// </summary>
    /// <param name="result"></param>
    private void OnGetFriendsConfirmedListSuccess(ExecuteCloudScriptResult result)
    {
        friends.Clear();

        if (result.FunctionResult != null)
        {

            Debug.Log(result.FunctionResult.ToString());
            // Access the Friends List from 'result.FunctionResult["Friends"]'
            string objectString = result.FunctionResult.ToString();


            //Patterns Regular Expressions
            string pattern = "\"Username\":\"(.*?)\"";
            string IdsPatter = "\"IDS\":\"(.*?)\"";
            string TagsPatter = "\"Tags\":\"(.*?)\"";

            MatchCollection matches = Regex.Matches(objectString, pattern);
            MatchCollection matche = Regex.Matches(objectString, IdsPatter);
            MatchCollection matchTag = Regex.Matches(objectString, TagsPatter);

            for (int i = 0; i < matches.Count; i++)
            {
                Friend newFriend = new Friend();
                newFriend.Username = matches[i].Groups[1].Value;
                newFriend.Id = matche[i].Groups[1].Value;
                newFriend.Tags = matchTag[i].Groups[1].Value;

                Friends.Add(newFriend);
            }

            friendList.InstanceFriendItem();
        }
    }

    // Callback for failed GetFriendsList 
    private void OnGetFriendsListFailure(PlayFabError error)
    {
        Debug.LogError("Failed to retrieve Friends List: " + error.ErrorMessage);
    }


    /// <summary>
    /// When you add a friend, the error messages and the right ones
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public bool ValidarUserNameFriend(string str)
    {
        validationMessage.color = Color.red;
        validationMessage.text = " ";
        panelMessageCheckName.SetActive(true);
        // Check if string is null or empty
        if (string.IsNullOrEmpty(str))
        {
            validationMessage.text = "El nombre de usuario no puede estar vacío";
            return false;
        }
        // Check if string starts with a space
        if (str.StartsWith(" "))
        {
            validationMessage.text = "El nombre de usuario no puede empezar por un espacio en blanco";
            return false;
        }
        // Check if string is only spaces
        if (str.Trim().Length == 0)
        {
            validationMessage.text = "El nombre de usuario no debe estar contenido solo por espacios";
            return false;
        }
        // Check minimum length
        if (str.Length < 3)
        {
            validationMessage.text = "El nombre de usuario debe contener más de 3 caracteres";
            return false;
        }
        // Check maximum length
        if (str.Length > 20)
        {
            validationMessage.text = "El nombre de usuario debe contener menos de 20 caracteres";
            return false;
        }
        // Check forbidden characters
        string forbidden = "!@#$%^&*()+=";
        foreach (char c in forbidden)
        {
            if (str.Contains(c))
            {
                validationMessage.text = "El nombre de usuario contiene un carácter no permitido";
                return false;
            }
        }
        // Check reserved words
        string[] reserved = { "admin", "root", "system" };
        if (reserved.Contains(str.ToLower()))
        {
            validationMessage.text = "El nombre de usuario no debe contener palabras restringidas";
            return false;
        }
        // String is valid
        validationMessage.color = Color.green;
        validationMessage.text = "Amigo añadido correctamente";
        return true;
    }

    //Clean the message panel
    public void CleanMessagePanel()
    {
        panelMessageCheckName.SetActive(false);
        validationMessage.text = " ";
    }
}