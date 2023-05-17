using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;



public class Achievements
{
    public bool interactAchievement;

    public Achievements(bool interactAchievement)
    {
        this.interactAchievement = interactAchievement;
    }
}


public class AchievementsManager
{
    public Achievements currentAchievements;

    // Save User Data
    public void SaveData(Achievements achievements)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Achievements", JsonConvert.SerializeObject(achievements)}
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
        if (result.Data != null && result.Data.ContainsKey("Achievements"))
        {
            currentAchievements = JsonConvert.DeserializeObject<Achievements>(result.Data["Achievements"].Value);
        }

    }


    public void OnError(PlayFabError obj)
    {
        Debug.Log("[PlayFab-ManageData] Error");
    }
}