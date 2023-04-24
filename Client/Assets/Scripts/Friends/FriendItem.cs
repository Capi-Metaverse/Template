using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendItem : MonoBehaviour
{
    private string username;
    private string id;

    public string Username { get => username; set => username = value; }
    public string Id { get => id; set => id = value; }

    public void Removefriends()
    {
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
        {
            FunctionName = "DenyFriendRequest",
            FunctionParameter = new { friendplayfabid = id },
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnRemoveFriendsSuccess, OnRemoveFriendsFailure);
    }

    // Callback for successful CloudScript function call
    private void OnRemoveFriendsSuccess(ExecuteCloudScriptResult result)
    {
        Debug.Log(result);
        Destroy(this.gameObject);
    }

    // Callback for failed CloudScript function call
    private void OnRemoveFriendsFailure(PlayFabError error)
    {
        Debug.LogError("Failed to retrieve Friends List: " + error.ErrorMessage);
    }
}
