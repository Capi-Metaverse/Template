using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendRequest : MonoBehaviour { 

public string ID;
FriendManager FriendManager;
void Start()
{

    FriendManager = gameObject.GetComponent<FriendManager>();
}

    public void acceptRequest()
    {
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
        {
            FunctionName = "AcceptFriendRequest",
            FunctionParameter = new
            {
                friendplayfabid = ID
            },
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnAddFriendSuccess, OnGetFriendsListFailure);
    }

    public void DenyRequest()
    {
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
        {
            FunctionName = "DenyFriendRequest",
            FunctionParameter = new
            {
                friendplayfabid = ID
            },
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnAddFriendSuccess, OnGetFriendsListFailure);
    }
    // Callback for successful CloudScript function call
    private void OnAddFriendSuccess(ExecuteCloudScriptResult result)
{
        Debug.Log("User added or denied successfully");
}

// Callback for failed CloudScript function call
private void OnGetFriendsListFailure(PlayFabError error)
{
    Debug.LogError("Failed to retrieve Friends List: " + error.ErrorMessage);
}
    
}
