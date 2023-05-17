using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Achievement{
    public string Name;
    public bool activate;

    public Achievement(string Name, bool activate)
    {
        this.Name = Name;
        this.activate = activate;
    }
}


public class AchievementsManager : MonoBehaviour 
{
    public AchivementList achivementList;
    public List<Achievement> currentAchievements;
    // Save User Data
    public void SaveData(List<Achievement> achievements)
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
        Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        if (result.Data != null && result.Data.ContainsKey("Achievements"))
        {
            currentAchievements = JsonConvert.DeserializeObject<List<Achievement>>(result.Data["Achievements"].Value);
            Debug.Log(currentAchievements[0].Name);
            if (!(SceneManager.GetSceneByName("Login") == SceneManager.GetActiveScene()))
            {
                achivementList.InstanceAchivementItem();
            }
        }

    }


    public void OnError(PlayFabError obj)
    {
        Debug.Log("[PlayFab-ManageData] Error");
    }
}