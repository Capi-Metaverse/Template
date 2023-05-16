using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using Newtonsoft.Json;
using static Fusion.NetworkCharacterController;

public class PlayerInteractionE : MonoBehaviour , IMetaEvent
{
    GameManager gameManager;
    public UserUIInfo data;

    GameObject _eventObject;
    GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }

    void HandleReadOnlyDataCallback(string value)
    {
        if (value != null)
        {
            // Do something with the retrieved value
            Debug.Log("Received value: " + value);
        }
        else
        {
            Debug.Log("Value not found or error occurred.");
        }
    }

    public void activate(bool host)
    {
        string playfabid =_eventObject.GetComponent<NetworkPlayer>().playfabIdentity;
        Debug.Log("PlayfabID del pulsado: " + playfabid);
        //get UIcard
        GetPublicDataFromOtherPlayer("76CEE0B9316C54B6", "userUICard", HandleReadOnlyDataCallback);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.FindInstance();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetPublicDataFromOtherPlayer(string otherPlayerId, string key, Action<string> callback)
    {
        var request = new GetUserDataRequest
        {
            PlayFabId = otherPlayerId,
            Keys = new List<string> { key }
        };

        PlayFabClientAPI.GetUserReadOnlyData(request, result =>
        {
            if (result.Data.TryGetValue(key, out var value))
            {
                // Do something with the retrieved value
                callback?.Invoke(value.Value);
                Debug.Log($"Retrieved value {value.Value} for key {key} from player {otherPlayerId}");
            }
            else
            {
                callback?.Invoke(null);
                Debug.Log($"Could not retrieve value for key {key} from player {otherPlayerId}");
            }
        }, error =>
        {
            Debug.LogError($"Error retrieving read-only data for player {otherPlayerId}: {error.ErrorMessage}");
        });
    }
}
