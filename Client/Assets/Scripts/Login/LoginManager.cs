using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;


public class LoginManager : MonoBehaviour
{
    [Header("UI")]
    //UI Text
    public TMP_Text messageText;

    //UI Inputs
    public TMP_InputField usernameInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    //UI Panels
    public GameObject LoginPanel;
    public GameObject RegisterPanel;
    public GameObject ResetPanel;

    //GameManager
    private GameManager gameManager;

    //Boolean that checks if the user is new or not
    private bool newUser = true;

    //Roles
    Dictionary<string, bool> roles = new Dictionary<string, bool>();
    string UserRolePlayFab;

    //Player data Classes
    [Serializable]
    public class PlayerDataUsername
    {
        public string getPlayerUsername;
    }
    public class PlayerDataId
    {
        public string getPlayerId;
    }

    private string actualRole = "none";


    private void Start()
    {
        //It gets the GameManager at the start
        gameManager = GameManager.FindInstance();

        //If there's an error in the connection it will return
        //This statement indicates to the user that an error has occurred.
        if (gameManager.GetConnectionStatus() == ConnectionStatus.Failed)
        {
            messageText.text = "An error has occurred. Try Again";
        }
    }





 //-------------------- Panel Modification Functions --------------------


    //Function that changes the UI to the Sign-Up UI
    public void ChangeRegister()
    {
        RegisterPanel.SetActive(true);
        ResetPanel.SetActive(false);
        LoginPanel.SetActive(false);
    }
    //Function that changes the UI to the Login UI
    public void ChangeLogin()
    {
        RegisterPanel.SetActive(false);
        ResetPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }

    //Function that changes the UI to the Reset Password UI
    public void ChangeReset()
    {
        RegisterPanel.SetActive(false);
        ResetPanel.SetActive(true);
        LoginPanel.SetActive(false);
    }




    /*Register Functions*/


    /// <summary>
    /// PlayFab. Confirms that the password reaches the length required and 
    /// sends a request to PlayFab to register the new user.
    /// </summary>
    public void RegisterButton()
    {
        if (!ValidateUserName(usernameInput.text))
        {
            return;
        }
        if (passwordInput.text.Length < 6)
        {
            messageText.color = Color.red;
            messageText.text = "Password too Short!";
            return;
        }

        //Playfab Request
        var request = new RegisterPlayFabUserRequest
        {
            Username = usernameInput.text,
            Email = emailInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = true
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSucess, OnError);
    }


    /// <summary>
    /// PlayFab. It's called when the user registration works correctly.
    /// </summary>
    /// <param name="result"></param>
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
    }

    /// <summary>
    /// PlayFab. It's called when the function that adds a member works.
    /// </summary>
    /// <param name="result"></param>
    private void OnAddMemberSuccess(ExecuteCloudScriptResult result)
    {
        Debug.Log("[PlayFab-LoginManager] Member added to the group successfully." + result.ToJson());

        //We register the achivements in Playfab
        List<Achievement> achievementList = AchivementRegister();

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Achievements", JsonConvert.SerializeObject(achievementList)}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnEndRegister, OnError);

    }

    private void OnEndRegister(UpdateUserDataResult obj)
    {
        //We change the UI
        messageText.text = "Registered!";
        LoginPanel.SetActive(true);
        RegisterPanel.SetActive(false);
    }




    /*Reset Password Functions*/


    /// <summary>
    /// PlayFab. Sends a request to PlayFab to reset the password.
    /// </summary>
    public void ResetPassword()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput.text,
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
        messageText.text = "Password reset mail sent!";
        LoginPanel.SetActive(true);
        ResetPanel.SetActive(false);
       
    }




    /*Login Functions*/


    /// <summary>
    /// PlayFab. Sends a request to PlayFab with the login values (Email, Password).
    /// </summary>
    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {

            Email = emailInput.text,
            Password = passwordInput.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    /// <summary>
    /// PlayFab. It's called when the Login works correctly.
    /// </summary>
    /// <param name="result"></param>
    void OnLoginSuccess(LoginResult result)
    {

        messageText.text = "logged in!";

        //Determine the role of the user
        //Confirm if admin
        ConfirmRole("admins");

    }

    /// <summary>
    /// PlayFab. Confirms if the user has the role introduced
    /// </summary>
    /// <param name="role"></param>
    public void ConfirmRole(string role)
    {
        actualRole = role;
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

        switch (actualRole)
        {
            case "admins": ConfirmRole("moderators"); break;
            case "moderators": ConfirmRole("clients"); break;
            case "clients": ConfirmRole("members"); break;
            case "members":
                {
                    //Obtain Name
                    var GetNa = new ExecuteCloudScriptRequest()
                    {
                        FunctionName = "getPlayerAccountInfoUsername"
                    };

                    PlayFabClientAPI.ExecuteCloudScript(GetNa, OnUsernameSuccess, OnError);


                    break;

                }
        }
    }
    /*ID and Username Functions*/

    /// <summary>
    /// PlayFab. It's called when the username has been returned succesfully. Sets the username value in the GameManager.
    /// </summary>
    /// <param name="result"></param>
    void OnUsernameSuccess(ExecuteCloudScriptResult result)
    {

        string jsonString = result.FunctionResult.ToString();

        PlayerDataUsername playerDataUsername = JsonUtility.FromJson<PlayerDataUsername>(jsonString);

        string username = playerDataUsername.getPlayerUsername;


        gameManager.SetUsername(username);
        gameManager.SetEmail(emailInput.text);

        //Obtain ID
        var GetID = new ExecuteCloudScriptRequest()
        {
            FunctionName = "getPlayerAccountInfoId"
        };

        PlayFabClientAPI.ExecuteCloudScript(GetID, OnIDSuccess, OnError);

        Debug.Log("[PlayFab-LoginManager] Username: " + username); // output: "prueba1"
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
        gameManager.SetUserID(IDMaster);

        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), AssignRole, OnError);
    }

   




    /*Add Member Functions*/


  

    private List<Achievement> AchivementRegister()
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

        if (result.Data != null && result.Data.ContainsKey("NewUser"))
        {
            newUser = Convert.ToBoolean(result.Data["NewUser"].Value);
        }


        UserRolePlayFab = roles.FirstOrDefault(x => x.Value == true).Key;
        switch (UserRolePlayFab)
        {
            case "admins":
                gameManager.SetUserRole(UserRole.Admin);
                break;
            case "members":
                gameManager.SetUserRole(UserRole.Employee);
                break;
            case "clients":
                gameManager.SetUserRole(UserRole.Client);
                break;
            case "moderators":
                gameManager.SetUserRole(UserRole.Moderator);
                break;
        }
        Debug.Log("[PlayFab-LoginManager] UserRole: " + gameManager.GetUserRole());

        //Change to the next scene
        if (newUser)
        {
            SceneManager.LoadSceneAsync("Tutorial");
            
        }
     
        else
        {
            SceneManager.LoadSceneAsync("Lobby");
        }
    }



    /*Error Callback*/

    
    /// <summary>
    /// PlayFab. It's called when an error is returned by one of the PlayFab Functions.
    /// </summary>
    /// <param name="error"></param>
    void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;
        Debug.Log("[PlayFab-LoginManager] Error:" + error.GenerateErrorReport());
    }

    /*Username Validation Message*/

    public bool ValidateUserName(string str)
    {
        // Check if string is null or empty
        if (string.IsNullOrEmpty(str))
        {
            NameErrorMessage("El nombre de usuario no puede estar vacío");
            return false;
        }
        // Check if string starts with a space
        if (str.StartsWith(" "))
        {
            NameErrorMessage("El nombre de usuario no puede empezar por un espacio en blanco");
            return false;
        }
        // Check if string is only spaces
        if (str.Trim().Length == 0)
        {
            NameErrorMessage("El nombre de usuario no debe estar contenido solo por espacios");
            return false;
        }
        // Check minimum length
        if (str.Length < 3)
        {
            NameErrorMessage("El nombre de usuario debe contener más de 3 caracteres");
            return false;
        }
        // Check maximum length
        if (str.Length > 20)
        {
            NameErrorMessage("El nombre de usuario debe contener menos de 20 caracteres");
            return false;
        }
        // Check forbidden characters
        string forbidden = "!@#$%^&*()+=";
        foreach (char c in forbidden)
        {
            if (str.Contains(c))
            {
                NameErrorMessage("El nombre de usuario contiene un carácter no permitido");
                return false;
            }
        }
        // Check reserved words
        string[] reserved = { "admin", "root", "system" };
        if (reserved.Contains(str.ToLower()))
        {
            NameErrorMessage("El nombre de usuario no debe contener palabras restringidas");
            return false;
        }
        // String is valid
        return true;
    }

    public void NameErrorMessage(string message)
    {
        messageText.fontSize = 4;
        messageText.color = Color.red;
        messageText.text = message;
    }
}