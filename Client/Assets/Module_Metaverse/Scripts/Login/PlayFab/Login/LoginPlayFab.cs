using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using static LoginManager;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using Fusion;

public class LoginPlayFab : MonoBehaviour, ILogin
{
    Dictionary<string, bool> roles = new Dictionary<string, bool>();
    private int requestsCounter = 0;
    string UserRolePlayFab;
    private bool newUser = true;

    public PanelLoginManager PanelLoginManager;

    private GameManager gameManager;
    private UserManager userManager;
    private MSceneManager mSceneManager;
    /// <summary>
    /// Method to login the user.
    /// </summary>
    /// <param name="emailInput"></param>
    /// <param name="passwordInput"></param>

    public void Start()
    {
        gameManager = GameManager.FindInstance();
        userManager = UserManager.FindInstance();
        mSceneManager = MSceneManager.FindInstance();
    }
    public void Login(string emailInput, string passwordInput)
    {
        PanelLoginManager.SetInfoMessage("Logging in...");
        var request = new LoginWithEmailAddressRequest
        {

            Email = emailInput,
            Password = passwordInput
        };
        userManager.Email = emailInput;
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    /// <summary>
    /// PlayFab. It's called when the Login works correctly.
    /// </summary>
    /// <param name="result"></param>
    void OnLoginSuccess(LoginResult result)
    {
        PanelLoginManager.SetInfoMessage("Correct Login");
        //Determine the role of the user
        ConfirmRole("Admin");
        ConfirmRole("Moderator");
        ConfirmRole("Client");
        ConfirmRole("Employee");

        var GetNa = new ExecuteCloudScriptRequest()
        {
            FunctionName = "getPlayerAccountInfoUsername"
        };

        PlayFabClientAPI.ExecuteCloudScript(GetNa, OnUsernameSuccess, OnError);

        //Obtain ID
        var GetID = new ExecuteCloudScriptRequest()
        {
            FunctionName = "getPlayerAccountInfoId"
        };

        PlayFabClientAPI.ExecuteCloudScript(GetID, OnIDSuccess, OnError);

    }
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

        PlayFabClientAPI.ExecuteCloudScript(ConfirmRole, OnRoleSuccess, OnError);
    }
    /// <summary>
    /// PlayFab. It's called when the confirmation of role of the user has been returned succesfully.
    /// </summary>
    /// <param name="result"></param>
    private void OnRoleSuccess(ExecuteCloudScriptResult result)
    {
        //Determine which role is being analyzed
        var Request = result.Request.ToJson().Split('"');
        var role = Request[Array.IndexOf(Request, "RoleId") + 2];

        //Confirm if the user has the role
        var functionresult = result.FunctionResult.ToString();
        var isRole = functionresult.Contains("true");

        roles.Add(role, isRole);

        if (roles.Count == 4) 
        {
            requestsCounter++;
            checkRequestCounter();
        }
    }

    private void checkRequestCounter()
    {
        if(requestsCounter == 3)
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest(), AssignRole, OnError);
        }

    }
    void OnUsernameSuccess(ExecuteCloudScriptResult result)
    {

        string jsonString = result.FunctionResult.ToString();

        PlayerDataUsername playerDataUsername = JsonUtility.FromJson<PlayerDataUsername>(jsonString);

        string username = playerDataUsername.getPlayerUsername;


      
        userManager.Username = username;
        requestsCounter++;
        checkRequestCounter();

        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnSensitivityCorrect, OnError);

    }

    private void OnSensitivityCorrect(GetUserDataResult result)
    {
        gameManager.Sensitivity = (float)Convert.ToDouble(result.Data["Sensitivity"].Value);
    }


    /// <summary>
    /// PlayFab. It's called when the MasterID is obtained succesfully.
    /// </summary>
    /// <param name="result"></param>
    private void OnIDSuccess(ExecuteCloudScriptResult result)
    {
        string jsonString = result.FunctionResult.ToString();

        PlayerDataId playerDataId = JsonUtility.FromJson<PlayerDataId>(jsonString);

        string IDMaster = playerDataId.getPlayerId;

        Debug.Log("[PlayFab-LoginManager] MasterID: " + IDMaster);
        //gameManager.SetUserID(IDMaster);
        userManager.UserID = IDMaster;
        requestsCounter++;
        checkRequestCounter();
    }
    private List<Achievement> AchievementRegister()
    {
        List<Achievement> ListaLogros = new List<Achievement>
        {
            new Achievement("Educated", false), //Finish the Tutorial.
            new Achievement("Dolled up", false), //Edit your profile for the first time.
            new Achievement("Independent", false), //Change your location for the first time.
            new Achievement("Traveler", false), //Change your location for the fifth time.
            new Achievement("Phileas Fogg", false), //Change your location for the 10 time.
            new Achievement("Brush Cleaner", false), //Draw for the first time.
            new Achievement("Artist", false), //Draw for the 5 time.
            new Achievement("Cultist", false), //Pray in Jose Photo
            new Achievement("Camera Assistant", false), //Do a recording
            new Achievement("Next Almodovar", false), //Do 10 recordings.
            new Achievement("Curious",false), //Interact with items 5 times.
            new Achievement("Restless", false), //Interact with items 15 times.
            new Achievement("Unhinged", false), //Interact with items 50 times.


        };

        return ListaLogros;
    }

    public void Register(string usernameInput, string emailInput, string passwordInput)
    {
        //Playfab Request
        var request = new RegisterPlayFabUserRequest
        {
            Username = usernameInput,
            Email = emailInput,
            Password = passwordInput,
            RequireBothUsernameAndEmail = true
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSucess, OnError);
    }

    /// <summary>
    /// PlayFab. Confirms that the password reaches the length required and 
    /// sends a request to PlayFab to register the new user.
    /// </summary>
    void OnRegisterSucess(RegisterPlayFabUserResult result)
    {
        //When the user is in the database, we assign their group


        //Add Member
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

        PanelLoginManager.ChangeLogin();
    }
    /// <summary>
    /// PlayFab. It's called when the function that adds a member works.
    /// </summary>
    /// <param name="result"></param>
    private void OnAddMemberSuccess(ExecuteCloudScriptResult result)
    {
        Debug.Log("[PlayFab-LoginManager] Member added to the group successfully." + result.ToJson());

        //We register the achivements in Playfab
        List<Achievement> achievementList = AchievementRegister();


        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Achievements", JsonConvert.SerializeObject(achievementList)},
                {"Sensitivity", "0.5"}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnEndRegister, OnError);

    }
    private void OnEndRegister(UpdateUserDataResult obj)
    {
        //We change the UI
       /* messageText.text = "Registered!";
        LoginPanel.SetActive(true);
        RegisterPanel.SetActive(false);*/
    }

    /// <summary>
    /// PlayFab. Sends a request to PlayFab to reset the password.
    /// </summary>
    /// <param name="emailInput"></param>
    public void OnReset(string emailInput)
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput,
            TitleId = "CB001",
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    /// <summary>
    /// PlayFab. It's called when the password reset mail is sent correctly.
    /// </summary>
    /// <param name="result"></param>
    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        string message = "Password reset mail sent!";
        PanelLoginManager.SetInfoMessage(message);
        PanelLoginManager.ChangeLogin();
    }

    /// <summary>
    /// PlayFab. It's called when the function that adds a member fails.
    /// </summary>
    /// <param name="error"></param>
    private void OnAddMemberFailure(PlayFabError error)
    {
        Debug.LogError("[PlayFab-LoginManager] Error adding member to group: " + error.ErrorMessage);
    }


    /// <summary>
    /// PlayFab. Assigns the role to the UserRole variable defined in GameManager
    /// </summary>
    public void AssignRole(GetUserDataResult result)
    {
        PanelLoginManager.SetInfoMessage("Confirming role");
        if (result.Data != null && result.Data.ContainsKey("NewUser"))
        {
            newUser = Convert.ToBoolean(result.Data["NewUser"].Value);
        }


        UserRolePlayFab = roles.FirstOrDefault(x => x.Value == true).Key;

        userManager.UserRole = (UserRole)Enum.Parse(typeof(UserRole), UserRolePlayFab);
    
       Debug.Log("[PlayFab-LoginManager] UserRole: " + userManager.UserRole);

        //Change to the next scene
        if (newUser)
        {
            mSceneManager.LoadTutorial();

        }

        else
        {
       
            mSceneManager.LoadScene("Lobby_Module");
        }
    }

    /// <summary>
    /// PlayFab. It's called when an error is returned by one of the PlayFab Functions.
    /// </summary>
    /// <param name="error"></param>
    void OnError(PlayFabError error)
    {
        PanelLoginManager.SetInfoMessage("Error");
        PanelLoginManager.SetErrorMessage(error.GenerateErrorReport());
        //messageText.text = error.ErrorMessage;
        Debug.Log("[PlayFab-LoginManager] Error:" + error.GenerateErrorReport());
    }
    /// <summary>
    /// PlayFab. Sends a request to PlayFab to reset the password.
    /// </summary>
 
}
