using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class InGameMessaging : MonoBehaviour
{
    public void SendMessage(string recipientId, string message)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "SendMessage",
            FunctionParameter = new { recipientId = recipientId, message = message }
        }, result =>
        {
            Debug.Log("Message sent to " + recipientId);
        }, error =>
        {
            Debug.LogError(error.GenerateErrorReport());
        });
    }

    /*public void GetConversations()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "GetConversations"
        }, result =>
        {
            // Update the UI with the list of conversations
            foreach (var conversation in result.FunctionResult.conversations)
            {
                // Display the conversation and its messages in the UI
            }
        }, error =>
        {
            Debug.LogError(error.GenerateErrorReport());
        });
    }*/
}

