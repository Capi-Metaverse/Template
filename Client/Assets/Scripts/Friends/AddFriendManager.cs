using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class AddFriendManager : MonoBehaviour
{
    public string friendUsername = "prueba12345"; // The username of the friend to be added

    public void AddFriend()
    {
        // Call the PlayFab Cloud Script function to add the friend
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
        {
            FunctionName = "AddFriend", // Replace with the name of your Cloud Script function
            FunctionParameter = new { friendUsername }, // Pass in any required parameters
            GeneratePlayStreamEvent = true // Set to true if you want PlayStream events to be generated for this API call
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnAddFriendSuccess, OnAddFriendFailure);
    }

    private void OnAddFriendSuccess(ExecuteCloudScriptResult result)
    {
        Debug.Log("Friend added successfully!");
        // Handle success, if necessary
    }

    private void OnAddFriendFailure(PlayFabError error)
    {
        Debug.LogError("Failed to add friend: " + error.ErrorMessage);
        // Handle failure, if necessary
    }
}