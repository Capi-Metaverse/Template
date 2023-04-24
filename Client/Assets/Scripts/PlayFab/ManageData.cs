using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;


public class InteractKey
{
    public int interactkey;

    public InteractKey(int interactkey)
    {
        this.interactkey = interactkey;
    }
}



public class PresentationKey
{
    public int presentation;

    public PresentationKey(int presentation)
    {
        this.presentation = presentation;
    }
}

public class WheelKey
{
    public int wheel;

    public WheelKey(int wheel)
    {
        this.wheel = wheel;
    }
}


public class Keys
{
    public int interact;
    public int presentationMode;
    public int wheel;

    public Keys(int interact, int presentationMode, int wheel)
    {
        this.interact = interact;
        this.presentationMode = presentationMode;
        this.wheel = wheel;
    }
}

public class ManageData : MonoBehaviour
{
    public Keys currentkeys;

    // Save User Data
    public void SaveData(Keys keys)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Keys", JsonConvert.SerializeObject(keys)}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    public void OnDataSend(UpdateUserDataResult obj)
    {
        Debug.Log("[PlayFab-ManageData] Data Sent");
    }


    //Load User Data
    public void LoadData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnCharactersDataReceived, OnError);
    }

    void OnCharactersDataReceived(GetUserDataResult result)
    {
        Debug.Log("[PlayFab-ManageData] Received characters data!");
        if (result.Data != null && result.Data.ContainsKey("Keys"))
        {
            currentkeys = JsonConvert.DeserializeObject<Keys>(result.Data["Keys"].Value);
        }
       
    }

 

    public void OnError(PlayFabError obj)
    {
        Debug.Log("[PlayFab-ManageData] Error");
    }
}
