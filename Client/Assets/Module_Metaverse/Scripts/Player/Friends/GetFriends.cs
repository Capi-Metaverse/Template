using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
    public Task<List<Friend>> GetFriendsConfirmedListAsync()
    {
        var tcs = new TaskCompletionSource<List<Friend>>();

        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
        {
            FunctionName = "getFriendsListRequester",
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(request, result =>
        {
            List<Friend> friends = OnGetFriendsConfirmedListSuccess(result);
            tcs.SetResult(friends);
        }, error =>
        {
            Debug.LogError("Failed to execute cloud script: " + error.ErrorMessage);
            tcs.SetResult(new List<Friend>()); // Set an empty list in case of failure
        });

        return tcs.Task;
    }

    /// <summary>
    /// Callback for successful GetFriendsList ,which gets the result of the callback and appends it to a list
    /// </summary>
    /// <param name="result"></param>
    private List<Friend> OnGetFriendsConfirmedListSuccess(ExecuteCloudScriptResult result)
    {
        List<Friend> friends = new List<Friend>();
        friendManager.Friends.Clear();

        if (result != null && result.FunctionResult != null)
        {
            string json = result.FunctionResult.ToString();

            // Deserialize the JSON response into a list of friend objects
            List<Friend> friendObjects = JsonConvert.DeserializeObject<List<Friend>>(json);
            friends.AddRange(friendObjects);

            //Patterns Regular Expressions
            string pattern = "\"Username\":\"(.*?)\"";
            string IdsPatter = "\"IDS\":\"(.*?)\"";
            string TagsPatter = "\"Tags\":\"(.*?)\"";

            MatchCollection matches = Regex.Matches(json, pattern);
            MatchCollection matche = Regex.Matches(json, IdsPatter);
            MatchCollection matchTag = Regex.Matches(json, TagsPatter);

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

        return friends;

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
