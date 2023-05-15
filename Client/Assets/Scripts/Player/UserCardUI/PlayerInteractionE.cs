using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using Newtonsoft.Json;

public class PlayerInteractionE : MonoBehaviour , IMetaEvent
{
    GameManager gameManager;
    public UserUIInfo data;

    GameObject _eventObject;
    GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }

    public void activate(bool host)
    {
        string playfabid =_eventObject.GetComponent<NetworkPlayer>().playfabIdentity;
        Debug.Log("PlayfabID del pulsado: " + playfabid);
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

    public void GetPublicDataFromOtherPlayer(string otherPlayerId, string key)
    {
        var request = new GetUserDataRequest
        {
            PlayFabId = otherPlayerId,
            Keys = new List<string> { key },
            IfChangedFromDataVersion = 0 // Optional: Only retrieve data if it has changed since a specific version
        };

        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnCharactersDataReceived, OnError);
    }

    void OnCharactersDataReceived(GetUserDataResult result)
    {
        Debug.Log("[PlayFab-ManageData] Received characters data!");
        if (result.Data != null && result.Data.ContainsKey("userUICard"))
        {
            data = JsonConvert.DeserializeObject<UserUIInfo>(result.Data["userUICard"].Value);
            Debug.Log(data);
        }

    }

    public void OnError(PlayFabError obj)
    {
        Debug.Log("[PlayFab-ManageData] Error");
    }
}
