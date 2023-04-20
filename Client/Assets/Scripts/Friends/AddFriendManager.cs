using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Unity.VisualScripting;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using System.Threading.Tasks;

public class AddFriendManager : MonoBehaviour
{
    public TMP_InputField inputFriendUsername; // The username of the friend to be added
    public GameObject panelMessageCheckName; // The panel of the validation message
    public TextMeshProUGUI validationMessage;// The validation message
    public List<string> listFriendsConfirmed;
    public List<string> listFriendsIdsConfirmed;
    public string userId = "";

    public void idPlayer()
    {
        if (ValidarUserNameFriend(inputFriendUsername.text))
        {
            var result = new GetAccountInfoRequest { Username = inputFriendUsername.text };

            PlayFabClientAPI.GetAccountInfo(result, OnGetAccountInfoSuccess, OnGetAccountInfoFailure);
        
        }
       
    }
    private void OnGetAccountInfoSuccess(GetAccountInfoResult result)
    {
        userId = result.AccountInfo.PlayFabId;
        Debug.Log("User ID: " + userId);
    }



    private void OnGetAccountInfoFailure(PlayFabError error)
    {
        Debug.LogError("GetAccountInfo failed: " + error.ErrorMessage);
    }
    public async void AddFriend()
    {
        if(ValidarUserNameFriend(inputFriendUsername.text))
        {
            idPlayer();
            Debug.Log(userId);
           while(userId.Equals(""))
            {
                await Task.Delay(100);
            }
          
            
            // Call the PlayFab Cloud Script function to add the friend
            ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
            {
                FunctionName = "SendFriendRequest", // Replace with the name of your Cloud Script function
                FunctionParameter = new { friendUsername = inputFriendUsername.text,
                                          friendplayfabid = userId
                }, // Pass in any required parameters
                GeneratePlayStreamEvent = true // Set to true if you want PlayStream events to be generated for this API call
            };

            PlayFabClientAPI.ExecuteCloudScript(request, OnAddFriendSuccess, OnAddFriendFailure);
        }
        userId = "";
        inputFriendUsername.text = null; // Clean the friend name field
    }
    private void OnAddFriendSuccess(ExecuteCloudScriptResult result)
    {
        Debug.Log(result.FunctionResult);
        // Handle success, if necessary
    }

    private void OnAddFriendFailure(PlayFabError error)
    {
        Debug.LogError("Failed to add friend: " + error.ErrorMessage);
        // Handle failure, if necessary
    }

    public void GetFriendsConfirmedList()
    {
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
        {
            FunctionName = "getFriendsListRequester",
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnGetFriendsConfirmedListSuccess, OnGetFriendsListFailure);
    }


    // Callback for successful CloudScript function call // llamamiento a la funcion getList de Cloud
    private void OnGetFriendsConfirmedListSuccess(ExecuteCloudScriptResult result)
    {
     listFriendsConfirmed.Clear();
     listFriendsIdsConfirmed.Clear();
        if (result.FunctionResult != null)
        {

            Debug.Log(result.FunctionResult.ToString());
            // Access the Friends List from 'result.FunctionResult["Friends"]'
            string objectString = result.FunctionResult.ToString();

            string pattern = "\"Username\":\"(.*?)\"";
            MatchCollection matches = Regex.Matches(objectString, pattern);

            foreach (Match match in matches)
            {
                if (match.Groups.Count > 1)
                {
                    string username = match.Groups[1].Value;
                    listFriendsConfirmed.Add(username);
                }
            }
           
            for (int i = 0; i < listFriendsConfirmed.Count; i++) {
                Debug.Log(listFriendsConfirmed[i].ToString());
            }

            string IdsPatter = "\"IDS\":\"(.*?)\"";
            MatchCollection matche = Regex.Matches(objectString, IdsPatter);

            foreach (Match match in matche)
            {
                if (match.Groups.Count > 1)
                {
                    string Ids = match.Groups[1].Value;
                    listFriendsIdsConfirmed.Add(Ids);
                }
            }
         
            for (int i = 0; i < listFriendsIdsConfirmed.Count; i++)
            {
                Debug.Log(listFriendsIdsConfirmed[i].ToString());
            }

        }
    }

    // Callback for failed CloudScript function call
    private void OnGetFriendsListFailure(PlayFabError error)
    {
        Debug.LogError("Failed to retrieve Friends List: " + error.ErrorMessage);
    }
    //Eliminar Amigos

    //Check if the friend name input is accurate
    public bool ValidarUserNameFriend(string str)
    {
        validationMessage.color = Color.red;
        validationMessage.text = " ";
        panelMessageCheckName.SetActive(true);
        // Check if string is null or empty
        if (string.IsNullOrEmpty(str))
        {
            validationMessage.text = "El nombre de usuario no puede estar vacío";
            return false;
        }
        // Check if string starts with a space
        if (str.StartsWith(" "))
        {
            validationMessage.text = "El nombre de usuario no puede empezar por un espacio en blanco";
            return false;
        }
        // Check if string is only spaces
        if (str.Trim().Length == 0)
        {
            validationMessage.text = "El nombre de usuario no debe estar contenido solo por espacios";
            return false;
        }
        // Check minimum length
        if (str.Length < 3)
        {
            validationMessage.text = "El nombre de usuario debe contener más de 3 caracteres";
            return false;
        }
        // Check maximum length
        if (str.Length > 20)
        {
            validationMessage.text = "El nombre de usuario debe contener menos de 20 caracteres";
            return false;
        }
        // Check forbidden characters
        string forbidden = "!@#$%^&*()+=";
        foreach (char c in forbidden)
        {
            if (str.Contains(c))
            {
                validationMessage.text = "El nombre de usuario contiene un carácter no permitido";
                return false;
            }
        }
        // Check reserved words
        string[] reserved = { "admin", "root", "system" };
        if (reserved.Contains(str.ToLower()))
        {
            validationMessage.text = "El nombre de usuario no debe contener palabras restringidas";
            return false;
        }
        // String is valid
        validationMessage.color = Color.green;
        validationMessage.text = "Amigo añadido correctamente";
        return true;
    }

    //Clean the message panel
    public void CleanMessagePanel()
    {
        panelMessageCheckName.SetActive(false);
        validationMessage.text = " ";
    }
}
