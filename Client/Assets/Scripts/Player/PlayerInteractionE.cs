using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayerInteractionE : MonoBehaviour , IMetaEvent
{
    GameManager gameManager;


    GameObject _eventObject;
    GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }

    public void activate(bool host)
    {
        string playfabid =_eventObject.GetComponent<NetworkPlayer>().playfabIdentity;
        //get UIcard
        GetPublicDataFromOtherPlayer(playfabid, "userUICard");
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

    public async void GetPublicDataFromOtherPlayer(string otherPlayerId, string key)
    {
        var request = new GetUserDataRequest
        {
            PlayFabId = otherPlayerId,
            Keys = new List<string> { key },
            IfChangedFromDataVersion = 0 // Optional: Only retrieve data if it has changed since a specific version
        };
        /*
        var result = await PlayFabClientAPI.GetUserDataAsync(request);

        if (result.Data.TryGetValue(key, out string value))
        {
            // Do something with the retrieved value
            Debug.Log($"Retrieved value {value} for key {key} from player {otherPlayerId}");
        }
        else
        {
            Debug.Log($"Could not retrieve value for key {key} from player {otherPlayerId}");
        }
        */
    }
}
