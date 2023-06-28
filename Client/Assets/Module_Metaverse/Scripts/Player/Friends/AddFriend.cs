using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddFriend : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputFriendUsername; // The username of the friend to be added
    [SerializeField] private GameObject panelMessageCheckName; // The panel of the validation message
    [SerializeField] private TextMeshProUGUI validationMessage;// The validation message
     private string userId = "";

    /// <summary>
    /// Method assigned to the Add Friend Button
    /// </summary>
    /// 
    public void AddFriendRequest()
    {
        RequestIDPlayer();
    }

    /// <summary>
    /// PlayFab - Method that gets the ID of a player from PlayFab
    /// </summary>
    public void RequestIDPlayer()
    {
        var result = new GetAccountInfoRequest { Username = inputFriendUsername.text };

        PlayFabClientAPI.GetAccountInfo(result, OnGetAccountInfoSuccess, OnGetAccountInfoFailure);

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
        validationMessage.text = error.ErrorMessage;
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
        validationMessage.text = "Friend request sended correctly";
        validationMessage.color = Color.green;
        // Handle success, if necessary
    }

    //Callback from a failed SendFriendRequest
    private void OnAddFriendFailure(PlayFabError error)
    {
        Debug.LogError("Failed to add friend: " + error.ErrorMessage);
        validationMessage.color = Color.red;
    }
}
