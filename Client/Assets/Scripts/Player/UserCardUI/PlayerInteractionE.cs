using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using static Fusion.NetworkCharacterController;
using PlayFab;

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
        GetPublicDataFromOtherPlayer("B8F668536256F472", "userUICard");
    }

    // Start is called before the first frame update
    void Start()
    {
        //gameManager = GameManager.FindInstance();
        
    }

    public void GetPublicDataFromOtherPlayer(string otherPlayerId, string key)
    {
        var request = new GetUserDataRequest
        {
            PlayFabId = otherPlayerId,
            Keys = new List<string> { key }
        };

        PlayFabClientAPI.GetUserData(request, OnUpdateUserDataSuccess, OnUpdateUserDataFailure);
       
    }

    private void OnUpdateUserDataSuccess(GetUserDataResult result)
    {
        Debug.Log("Rsult to user successfully!");
        Debug.Log(result.Data["userUICard"].Value);
    }

    private void OnUpdateUserDataFailure(PlayFabError error)
    {
        Debug.LogError("Failed to upload image to user: " + error.GenerateErrorReport());
    }

}