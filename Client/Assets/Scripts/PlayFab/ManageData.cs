using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;


public class Keys
{
    public int interact;
    public int presentationMode;

    public Keys(int interact, int presentationMode)
    {
        this.interact = interact;
        this.presentationMode = presentationMode;
    }
}

public class ManageData : MonoBehaviour
{
    public Keys currentkeys;

    // Save User Data
    public void SaveData()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Keys", JsonConvert.SerializeObject(new Keys(102, 108))}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    public void OnDataSend(UpdateUserDataResult obj)
    {
        Debug.Log("Data Sent");
    }


    //Load User Data
    public void LoadData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnCharactersDataReceived, OnError);
    }

    void OnCharactersDataReceived(GetUserDataResult result)
    {
        Debug.Log("Received characters data!");
        if (result.Data != null && result.Data.ContainsKey("Keys"))
        {
            currentkeys = JsonConvert.DeserializeObject<Keys>(result.Data["Keys"].Value);
    
            
        }
       
    }
    public void returncurrentkeys()
    {
        Debug.Log(currentkeys.interact);
        Debug.Log(currentkeys.presentationMode);
    }

 

    public void OnError(PlayFabError obj)
    {
        Debug.Log("Error");
    }
}
