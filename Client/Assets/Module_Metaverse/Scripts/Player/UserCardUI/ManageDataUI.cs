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
/// Class and constructor UserUIInfo
/// </summary>

namespace UserCard
{
    public class UserUIInfo
    {
        public string name;
        public string teams;
        public string about;
        public string hobbies;
        public string CV;



        public UserUIInfo(string name, string teams, string about, string hobbies, string CV)
        {
            this.name = name;
            this.teams = teams;
            this.about = about;
            this.hobbies = hobbies;
            this.CV = CV;
        }
    }
    public class ManageDataUI
    {
        public UserUIInfo data;

        /// <summary>
        /// Save User Data with key userUICard
        /// </summary>
        /// <param name="data"></param>
        public void SaveData(UserUIInfo data)
        {
            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
            {
                {"userUICard", JsonConvert.SerializeObject(data)}
            },
                Permission = UserDataPermission.Public
            };

            PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
        }

        public void OnDataSend(UpdateUserDataResult obj)
        {
            Debug.Log("[PlayFab-ManageData] Data Sent");
        }


        /// <summary>
        /// Load User Data
        /// </summary>
        public void LoadData()
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnCharactersDataReceived, OnError);
        }
        /// <summary>
        /// If the user receives the data, he stores it in a variable
        /// </summary>
        /// <param name="result"></param>
        void OnCharactersDataReceived(GetUserDataResult result)
        {
            Debug.Log("[PlayFab-ManageData] Received characters data!");

            if (result.Data != null && result.Data.ContainsKey("userUICard"))
            {
                Debug.Log(result.Data["userUICard"].Value);
                data = JsonConvert.DeserializeObject<UserUIInfo>(result.Data["userUICard"].Value);
            }

        }

        public void OnError(PlayFabError obj)
        {
            Debug.Log("[PlayFab-ManageData] Error");
        }
    }

}