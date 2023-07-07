using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;

public class ControlSensitivityPlayFab : MonoBehaviour
{
    public float sensitivityValue;

    public void ChangeSensitivity(float value)
    {
        sensitivityValue = value;
        UpdateUserDataRequest request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Sensitivity", sensitivityValue.ToString()}
            }
        };

        PlayFabClientAPI.UpdateUserData(request, OnUpdateUserSensitivity, OnError);
    }

    private void OnUpdateUserSensitivity(UpdateUserDataResult result)
    {
        Debug.Log("Sensitivity function correct");
    }

    private void OnError(PlayFabError error)
    {
        Debug.Log("Sensitivity function error : " + error.GenerateErrorReport());
    }
}
