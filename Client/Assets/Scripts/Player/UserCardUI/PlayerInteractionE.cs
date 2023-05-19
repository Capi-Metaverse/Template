using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using static Fusion.NetworkCharacterController;
using PlayFab;
using Newtonsoft.Json;

public class PlayerInteractionE : MonoBehaviour , IMetaEvent
{
    GameManager gameManager;
    public UserUIInfo data;
    public GameObject card;
    public GameObject apagar;
    public VisionData visionData;

    CharacterInputHandler characterInputHandler;
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

    private void LoadDataIntoCard(GetUserDataResult result)
    {
        card.SetActive(true);
        apagar.SetActive(false);
        characterInputHandler = GameManager.FindInstance().GetCurrentPlayer().gameObject.GetComponent<CharacterInputHandler>();
        characterInputHandler.DeactivateALL();

        data = JsonConvert.DeserializeObject<UserUIInfo>(result.Data["userUICard"].Value);
        visionData = card.GetComponent<VisionData>();

        visionData.UserNameTitle.text = data.name;
        visionData.TemasText.text = data.teams;
        visionData.OboutText.text = data.about;
        visionData.HobbiesText.text = data.hobbies;
        visionData.CVText.text = data.CV;
    }

    private void OnUpdateUserDataSuccess(GetUserDataResult result)
    {
        Debug.Log("Rsult to user successfully!");
        Debug.Log(result.Data["userUICard"].Value);
        LoadDataIntoCard(result);
    }

    private void OnUpdateUserDataFailure(PlayFabError error)
    {
        Debug.LogError("Failed to upload image to user: " + error.GenerateErrorReport());
    }

}