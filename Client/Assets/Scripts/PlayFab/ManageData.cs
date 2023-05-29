using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

/// <summary>
/// Class and construvtor to InteractKey
/// </summary>
public class InteractKey
{
    public int interactkey;

    public InteractKey(int interactkey)
    {
        this.interactkey = interactkey;
    }
}


/// <summary>
/// Class and construvtor to PresentationKey
/// </summary>
public class PresentationKey
{
    public int presentation;

    public PresentationKey(int presentation)
    {
        this.presentation = presentation;
    }
}

/// <summary>
/// Class and construvtor to WheelKey
/// </summary>
public class WheelKey
{
    public int wheel;

    public WheelKey(int wheel)
    {
        this.wheel = wheel;
    }
}

/// <summary>
/// Class and construvtor to Keys
/// </summary>
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
/// <summary>
/// Save Data with the Key Keys
/// </summary>
public class ManageData 
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
    /// <summary>
    /// Set the value to currentKeys
    /// </summary>
    /// <param name="result"></param>
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
