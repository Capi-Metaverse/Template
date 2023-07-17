using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Friends;
using Player;
using Unity.Entities.UniversalDelegates;

public class GetFriendMinimap : MonoBehaviour
{
    [SerializeField] private FriendManager friendManager;
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

        if (result != null && result.FunctionResult != null)
        {
            string json = result.FunctionResult.ToString();

            // Deserialize the JSON response into a list of friend objects
            List<Friend> friendObjects = JsonConvert.DeserializeObject<List<Friend>>(json);
            for (int i = 0; i < friendObjects.Count; i++)
            {
                Debug.Log("Sin filtrar" + friendObjects[i]);
                if (friendObjects[i].Tags == "confirmed")
                {
                    Debug.Log("Filtrado" + friendObjects[i]);
                    friends.Add(friendObjects[i]);
                }
            }

            
        }
        return friends;
    }
}
