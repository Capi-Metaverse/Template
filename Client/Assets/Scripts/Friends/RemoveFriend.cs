using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveFriend : MonoBehaviour
{
    public string Username;
    public string ID;
    AddFriendManager Addfriendmanager;
    void Start()
    {

        Addfriendmanager = gameObject.GetComponent<AddFriendManager>();
    }

    public void Removefriends()
    {
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
        {
            FunctionName = "RemoveFriends",
            FunctionParameter = new { RemoveFriendId = ID },
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
