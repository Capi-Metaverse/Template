using PlayFab;
using PlayFab.ClientModels;

using UnityEngine;

using TMPro;

using EntityKey = PlayFab.GroupsModels.EntityKey;

using UnityEngine.SceneManagement;
using Fusion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class LoginManager : MonoBehaviour
{
    public TMP_Text messageText;
    public TMP_InputField usernameInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    private GameManager gameManager;
    ManageData manageData;
    private EntityKey groupAdminEntity;

    //Variable related to roles
    Dictionary<string, bool> roles = new Dictionary<string, bool>();
    public string UserRole;

    [System.Serializable]
    public class PlayerDataUsername
    {
        public string getPlayerUsername;
    }
    public class PlayerDataId
    {
        public string getPlayerId;
    }
    private void Start()
    {
        gameManager = GameManager.FindInstance();
    }


    //Todo el tema de los paneles
    public GameObject LoginPanel;
    public GameObject RegisterPanel;
    public GameObject ResetPanel;


    public void ChangeRegister()
    {
        RegisterPanel.SetActive(true);
        ResetPanel.SetActive(false);
        LoginPanel.SetActive(false);
    }
    public void ChangeLogin()
    {
        RegisterPanel.SetActive(false);
        ResetPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }
    public void ChangeReset()
    {
        RegisterPanel.SetActive(false);
        ResetPanel.SetActive(true);
        LoginPanel.SetActive(false);
    }



    //Función register
    public void RegisterButton()
    {
        if (passwordInput.text.Length < 6)
        {
            messageText.text = "Password too Short!";
            return;
        }
        var request = new RegisterPlayFabUserRequest
        {
            Username = usernameInput.text,
            Email = emailInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = true
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSucess, OnError);
    }
    //Cuando Register Funciona
    void OnRegisterSucess(RegisterPlayFabUserResult result)
    {
        messageText.text = "Registered and logged in!";
        LoginPanel.SetActive(true);
        RegisterPanel.SetActive(false);

        //Añadir miembro
        var AddMem = new ExecuteCloudScriptRequest()
        {
            FunctionName = "addMember",
            FunctionParameter = new
            {
                GroupId = "77569033BA83F38B",
            },
            //GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(AddMem, OnAddMemberSuccess, OnAddMemberFailure);
    }  
      


    public void ResetPassword()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput.text,
            TitleId = "CB001",
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        messageText.text = "Password reset mail sent!";
        LoginPanel.SetActive(true);
        ResetPanel.SetActive(false);
       
    }

    //Funcion Login
    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {

            Email = emailInput.text,
            Password = passwordInput.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    //Cuando Login Funciona
    void OnLoginSuccess(LoginResult result)
    {

     
       
        messageText.text = "logged in!";


        //Obtener Nombre
        var GetNa = new ExecuteCloudScriptRequest()
        {
            FunctionName = "getPlayerAccountInfoUsername"
        };

        PlayFabClientAPI.ExecuteCloudScript(GetNa, OnUsernameSuccess, OnError);

        //Obtener ID
        var GetID = new ExecuteCloudScriptRequest()
        {
            FunctionName = "getPlayerAccountInfoId"
        };

        PlayFabClientAPI.ExecuteCloudScript(GetID, OnIDSuccess, OnError);

        //Detemine the role of the user
        StartCoroutine(DetermineRole());

        //The load scene is inside determineRole for now
    }

    //Cuando Obtener ID funciona
    private void OnIDSuccess(ExecuteCloudScriptResult result)
    {
        string jsonString = result.FunctionResult.ToString();

        PlayerDataId playerDataId = JsonUtility.FromJson<PlayerDataId>(jsonString);

        string IDMaster = playerDataId.getPlayerId;

        Debug.Log("El Master ID es = " + IDMaster);
        gameManager.userID = IDMaster;
    }
    //Cuando añadir miembtro funciona
    private void OnAddMemberSuccess(ExecuteCloudScriptResult result)
    {
        Debug.Log("Member added to group successfully." + result.ToJson());
    }
    //Cuando añadir miembtro NO funciona
    private void OnAddMemberFailure(PlayFabError error)
    {
        Debug.LogError("Error adding member to group: " + error.ErrorMessage);
    }

    /*------------------------------------------------------------------------*/

    IEnumerator DetermineRole()
    {
        //Confirm if admin
        ConfirmRole("admins");

        //Confirm if moderator
        ConfirmRole("moderators");

        //Confirm if client
        ConfirmRole("clients");

        //Confirm if employee
        ConfirmRole("members");

        yield return new WaitForSeconds(4);

        gameManager.UserRole = roles.FirstOrDefault(x => x.Value == true).Key;
        Debug.Log("UserRole: " + gameManager.UserRole);

        //Change to the next scene
        SceneManager.LoadSceneAsync("Lobby");
    }

    //Comfirm which is the role of the user
    public void ConfirmRole(string role)
    {
        var ConfirmRole = new ExecuteCloudScriptRequest()
        {
            FunctionName = "checkRole",
            FunctionParameter = new
            {
                GroupId = "77569033BA83F38B",
                RoleId = role
            }
        };

        PlayFabClientAPI.ExecuteCloudScript(ConfirmRole, OnAdmin, OnError);
    }

    //Confirm which role has the user
    private void OnAdmin(ExecuteCloudScriptResult result)
    {
        //Determine which role is being analyzed
        var Request = result.Request.ToJson().Split('"');
        var role = Request[Array.IndexOf(Request, "RoleId") + 2];

        //Confirm if the user has the role
        var functionresult = result.FunctionResult.ToString();
        var isRole = functionresult.Contains("true");

        roles.Add(role, isRole);
    }


    /*------------------------------------------------------------------------*/

    //Cuando Username funciona
    void OnUsernameSuccess(PlayFab.ClientModels.ExecuteCloudScriptResult result)
    {

        //Debug.Log(result.FunctionResult);



        string jsonString = result.FunctionResult.ToString();

        PlayerDataUsername playerDataUsername = JsonUtility.FromJson<PlayerDataUsername>(jsonString);

        string username = playerDataUsername.getPlayerUsername;

        gameManager.username = username;

        Debug.Log("El Master Nombre es = " + username); // output: "prueba1"


    }
    //Error General

    void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;
        Debug.Log(error.GenerateErrorReport());
    }
    void OnSucces(ExecuteCloudScriptResult result)
    {
        Debug.Log(result.ToJson());
    }
}