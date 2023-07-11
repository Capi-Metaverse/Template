using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script is used to give properties to the prefac of friends.
/// </summary>
namespace Friends
{
    public class FriendItem : MonoBehaviour
    {
        private string username;
        private string id;

        public string Username { get => username; set => username = value; }
        public string Id { get => id; set => id = value; }
        /// <summary>
        /// PlayFab - to remove from the list of confirmed friends
        /// </summary>
        public void Removefriends()
        {
            ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
            {
                FunctionName = "DenyFriendRequest",
                FunctionParameter = new { friendplayfabid = id },
                GeneratePlayStreamEvent = true
            };

            PlayFabClientAPI.ExecuteCloudScript(request, OnRemoveFriendsSuccess, OnRemoveFriendsFailure);
        }

        /// <summary>
        /// Friend Remove Success
        /// </summary>
        /// <param name="result"></param>
        private void OnRemoveFriendsSuccess(ExecuteCloudScriptResult result)
        {
            Debug.Log(result);
            Destroy(this.gameObject);
        }
        /// <summary>
        /// Friend Remove Fail
        /// </summary>
        /// <param name="error"></param>
        private void OnRemoveFriendsFailure(PlayFabError error)
        {
            Debug.LogError("Failed to retrieve Friends List: " + error.ErrorMessage);
        }
    }
}
