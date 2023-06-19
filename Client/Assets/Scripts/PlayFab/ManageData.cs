using System.Collections.Generic;
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
/// Class and construvtor to openMiniMapKey
/// </summary>
public class OpenMiniMapKey
{
    public int openMiniMapKey;

    public OpenMiniMapKey(int openMiniMapKey)
    {
        this.openMiniMapKey = openMiniMapKey;
    }
}

/// <summary>
/// Class and construvtor to muteVoiceKey
/// </summary>
public class MuteVoiceKey
{
    public int muteVoiceKey;

    public MuteVoiceKey(int muteVoiceKey)
    {
        this.muteVoiceKey = muteVoiceKey;
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
    public int openMiniMap;
    public int muteVoice;

    public Keys(int interact, int presentationMode, int wheel, int openMiniMap, int muteVoice)
    {
        this.interact = interact;
        this.presentationMode = presentationMode;
        this.wheel = wheel;
        this.openMiniMap = openMiniMap;
        this.muteVoice = muteVoice;
    }
}
/// <summary>
/// Save Data with the Key Keys
/// </summary>
public class ManageData
{
    public Keys currentkeys;

    // Save User current keys data to playfab
    public void SaveCurrentKeysDataPlayfab(Keys keys)
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
        Debug.Log("[PlayFab-ManageData] Current keys data Sent succesfully");
    }

    /// <summary>
    /// Get current keys data from playfab
    /// </summary>
    /// <param name="result"></param>
    public void GetCurrentKeysDataPlayfab()
    {
        var request = new GetUserDataRequest
        {
            Keys = new List<string> { "Keys" }
        };

        PlayFabClientAPI.GetUserData(request, OnCharactersDataReceived, OnError);
    }

    /// <summary>
    /// Fills the field currentKeys with the retrieved keys from playfab
    /// </summary>
    /// <param name="result"></param>
    void OnCharactersDataReceived(GetUserDataResult result)
    {
        Debug.Log("[PlayFab-ManageData] Received current keys on playfab data!");
        if (result.Data != null && result.Data.ContainsKey("Keys"))
        {
            currentkeys = JsonConvert.DeserializeObject<Keys>(result.Data["Keys"].Value);
        }
    }

    /// <summary>
    /// Inform that an error ocurred getting the current keys data from playfab
    /// </summary>
    /// <param name="result"></param>
    public void OnError(PlayFabError obj)
    {
        Debug.Log("[PlayFab-ManageData] Error retrieving current keys data");
    }
}
