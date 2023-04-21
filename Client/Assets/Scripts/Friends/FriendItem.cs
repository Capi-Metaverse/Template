using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendItem : MonoBehaviour
{
    public string Username;
    public string ID;
    FriendManager FriendManager;
    void Start()
    {

        FriendManager = gameObject.GetComponent<FriendManager>();
    }

    public void Removefriends()
    {
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
        {
            FunctionName = "DenyFriendRequest",
            FunctionParameter = new { friendplayfabid = ID },
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
