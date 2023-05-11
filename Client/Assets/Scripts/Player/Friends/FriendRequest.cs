using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendRequest : MonoBehaviour { 

    private string id;

    [SerializeField] private FriendList friendList;

    public string Id { get => id; set => id = value; }

    public void AcceptRequest()
    {
        friendList = GameObject.Find("TabFriends").GetComponent<FriendList>();

        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
        {
            FunctionName = "AcceptFriendRequest",
            FunctionParameter = new
            {
                friendplayfabid = id
            },
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnAddFriendSuccess, OnAddFriendFailure);

        friendList.InstanceFriendItem();
    }

    public void DenyRequest()
    {
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
        {
            FunctionName = "DenyFriendRequest",
            FunctionParameter = new
            {
                friendplayfabid = id
            },
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnAddFriendSuccess, OnAddFriendFailure);
    }
    // Callback for successful CloudScript function call
    private void OnAddFriendSuccess(ExecuteCloudScriptResult result)
{
        Debug.Log("User added or denied successfully");
        Destroy(this.gameObject);
}

// Callback for failed CloudScript function call
private void OnAddFriendFailure(PlayFabError error)
{
    Debug.LogError("Failed to retrieve Friends List: " + error.ErrorMessage);
    Destroy(this.gameObject);
    }
    
}
