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

/// <summary>
/// Class and Constructor to Achievement
/// </summary>
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
    public AchievementList achivementList;
    public List<Achievement> currentAchievements;
    
    /// <summary>
    /// PlayFab - Save the Data in Json, with the Key Achivements
    /// </summary>
    /// <param name="achievements"></param>
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


    /// <summary>
    /// Load the data from Own User
    /// </summary>
    public void LoadData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnCharactersDataReceived, OnError);
    }

    /// <summary>
    /// Receives the data and filters it to retrieve only those containing the Key Achievements.
    /// </summary>
    /// <returns>
    /// List How content the Key Achievements
    /// </returns>
    /// <param name="result"></param>
    void OnCharactersDataReceived(GetUserDataResult result)
    {
        if (result.Data != null && result.Data.ContainsKey("Achievements"))
        {
            currentAchievements = JsonConvert.DeserializeObject<List<Achievement>>(result.Data["Achievements"].Value);
            Debug.Log(currentAchievements[0].Name);
           
        }

    }


    public void OnError(PlayFabError obj)
    {
        Debug.Log("[PlayFab-ManageData] Error");
    }
}