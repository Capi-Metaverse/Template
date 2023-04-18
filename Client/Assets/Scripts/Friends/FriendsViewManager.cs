using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Unity.VisualScripting;
using TMPro;
using System.Collections.Generic;

public class FriendsViewManager : MonoBehaviour
{
    public GameObject FriendPrefab;
    List<string> NamePlayer = new List<string>();

    private void OnEnable()
    {
        
    }

    public void GetFriendsList()
    {
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
        {
            FunctionName = "getFriendsList",
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnGetFriendsListSuccess, OnGetFriendsListFailure);
    }

    // Callback for successful CloudScript function call
    private void OnGetFriendsListSuccess(ExecuteCloudScriptResult result)
    {

        if (result.FunctionResult != null)
        {
            Debug.Log(result.FunctionResult);
            // Access the Friends List from 'result.FunctionResult["Friends"]'
        }
    }

    // Callback for failed CloudScript function call
    private void OnGetFriendsListFailure(PlayFabError error)
    {
        Debug.LogError("Failed to retrieve Friends List: " + error.ErrorMessage);
    }
}