using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Unity.VisualScripting;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

public class AddFriendManager : MonoBehaviour
{
    public TMP_InputField inputFriendUsername; // The username of the friend to be added
    public List<string> listFriends;
    public List<string> listFriendsIds;


    public void AddFriend()
    {
        // Call the PlayFab Cloud Script function to add the friend
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
        {
            FunctionName = "AddFriend", // Replace with the name of your Cloud Script function
            FunctionParameter = new { friendUsername = inputFriendUsername.text }, // Pass in any required parameters
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
    public void GetFriendsList()
    {
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
        {
            FunctionName = "getFriendsList",
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnGetFriendsListSuccess, OnGetFriendsListFailure);
    }


    // Callback for successful CloudScript function call // llamamiento a la funcion getList de Cloud
    private void OnGetFriendsListSuccess(ExecuteCloudScriptResult result)
    {
     listFriends.Clear();
        if (result.FunctionResult != null)
        {

            Debug.Log(result.FunctionResult.ToString());
            // Access the Friends List from 'result.FunctionResult["Friends"]'
            string objectString = result.FunctionResult.ToString();

            string pattern = "\"Username\":\"(.*?)\"";
            MatchCollection matches = Regex.Matches(objectString, pattern);

            foreach (Match match in matches)
            {
                if (match.Groups.Count > 1)
                {
                    string username = match.Groups[1].Value;
                    listFriends.Add(username);
                }
            }
            //listFriends = (objectString.Replace("[", "").Replace("]", "").Replace("\"", "").Split("").ToList());
            for (int i = 0; i < listFriends.Count; i++) {
                Debug.Log(listFriends[i].ToString());
            }

            string IdsPatter = "\"IDS\":\"(.*?)\"";
            MatchCollection matche = Regex.Matches(objectString, IdsPatter);

            foreach (Match match in matche)
            {
                if (match.Groups.Count > 1)
                {
                    string Ids = match.Groups[1].Value;
                    listFriendsIds.Add(Ids);
                }
            }
            //listFriends = (objectString.Replace("[", "").Replace("]", "").Replace("\"", "").Split("").ToList());
            for (int i = 0; i < listFriendsIds.Count; i++)
            {
                Debug.Log(listFriendsIds[i].ToString());
            }

        }
    }

    // Callback for failed CloudScript function call
    private void OnGetFriendsListFailure(PlayFabError error)
    {
        Debug.LogError("Failed to retrieve Friends List: " + error.ErrorMessage);
    }
    //Eliminar Amigos
}
