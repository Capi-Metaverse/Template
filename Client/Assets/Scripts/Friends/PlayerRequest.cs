using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRequest : MonoBehaviour { 

public string ID;
AddFriendManager Addfriendmanager;
void Start()
{

    Addfriendmanager = gameObject.GetComponent<AddFriendManager>();
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
        Debug.Log("YEAAAAAAA");
}

// Callback for failed CloudScript function call
private void OnGetFriendsListFailure(PlayFabError error)
{
    Debug.LogError("Failed to retrieve Friends List: " + error.ErrorMessage);
}
    
}
