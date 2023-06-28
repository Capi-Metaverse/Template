using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class GetFriends : MonoBehaviour
{
    private FriendList friendList;
    [SerializeField] private FriendManager friendManager;

    private void Start()
    {
       friendList = gameObject.GetComponent<FriendList>();
    }
    /// <summary>
    /// PlayFab - Method that returns the list of friends from Playfab
    /// </summary>
    public void GetFriendsConfirmedList()
    {
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
        {
            FunctionName = "getFriendsListRequester",
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnGetFriendsConfirmedListSuccess, OnGetFriendsListFailure);
    }

    /// <summary>
    /// Callback for successful GetFriendsList ,which gets the result of the callback and appends it to a list
    /// </summary>
    /// <param name="result"></param>
    private void OnGetFriendsConfirmedListSuccess(ExecuteCloudScriptResult result)
    {
        friendManager.Friends.Clear();

        if (result.FunctionResult != null)
        {

            Debug.Log(result.FunctionResult.ToString());
            // Access the Friends List from 'result.FunctionResult["Friends"]'
            string objectString = result.FunctionResult.ToString();


            //Patterns Regular Expressions
            string pattern = "\"Username\":\"(.*?)\"";
            string IdsPatter = "\"IDS\":\"(.*?)\"";
            string TagsPatter = "\"Tags\":\"(.*?)\"";

            MatchCollection matches = Regex.Matches(objectString, pattern);
            MatchCollection matche = Regex.Matches(objectString, IdsPatter);
            MatchCollection matchTag = Regex.Matches(objectString, TagsPatter);

            for (int i = 0; i < matches.Count; i++)
            {
                Friend newFriend = new Friend();
                newFriend.Username = matches[i].Groups[1].Value;
                newFriend.Id = matche[i].Groups[1].Value;
                newFriend.Tags = matchTag[i].Groups[1].Value;

                friendManager.Friends.Add(newFriend);
            }

            friendList.InstanceFriendItem();
        }

    }

    // Callback for failed GetFriendsList 
    private void OnGetFriendsListFailure(PlayFabError error)
    {
        Debug.LogError("Failed to retrieve Friends List: " + error.ErrorMessage);

    }


    /// <summary>
    /// When you add a friend, the error messages and the right ones
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>

    //Clean the message panel
    /*public void CleanMessagePanel()
    {
        panelMessageCheckName.SetActive(false);
        validationMessage.text = " ";
    }*/
}
