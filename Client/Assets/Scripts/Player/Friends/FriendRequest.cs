using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendRequest : MonoBehaviour { 

    private string id;

    [SerializeField] private FriendList friendList;

    public string Id { get => id; set => id = value; }

    /// <summary>
    /// PlayFab - Accept Friend Request
    /// </summary>
    public void AcceptRequest()
    {
        friendList = gameObject.GetComponentInParent<FriendList>();
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
        
    }
    /// <summary>
    /// PlayFab - Deny Friend Request
    /// </summary>
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

    /// <summary>
    /// Callback for successful CloudScript function call
    /// </summary>

    private void OnAddFriendSuccess(ExecuteCloudScriptResult result)
{
        friendList.InstanceFriendItem();
        Destroy(this.gameObject);
}

    /// <summary>
    /// Callback for Failed CloudScript function call
    /// </summary>
    private void OnAddFriendFailure(PlayFabError error)
{
    Debug.LogError("Failed to retrieve Friends List: " + error.ErrorMessage);
    Destroy(this.gameObject);
    }
    
}
