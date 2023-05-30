using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public Dictionary<string, KeyCode> buttonKeys;
    public ManageData manageData;
    private PlayerUiPrefab playerUiPrefab;

    public Keys currentKeys;
    /// <summary>
    /// Detect if playfab has default values and if not, create default values, and update them in playfab.
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadInitialKeys()
    {
        // Do some work here...
        yield return null; // Wait one frame

        // Do some more work here...
        yield return new WaitForSeconds(4); // Wait for 4 seconds
        
        buttonKeys = new Dictionary<string, KeyCode>();
        if (manageData.currentkeys == null)
        {
            buttonKeys["Interact"] = KeyCode.E;
            buttonKeys["ChangeCamera"] = KeyCode.K;
            buttonKeys["Wheel"] = KeyCode.B;

            manageData.currentkeys = new Keys((int)KeyCode.E, (int)KeyCode.K, (int)KeyCode.B);
            manageData.SaveData(manageData.currentkeys);
        }
        else
        {
            //Assign key values from PlayFab
            buttonKeys["Interact"] = (KeyCode)manageData.currentkeys.interact;
            buttonKeys["ChangeCamera"] = (KeyCode)manageData.currentkeys.presentationMode;
            buttonKeys["Wheel"] = (KeyCode)manageData.currentkeys.wheel;

        }
        currentKeys = manageData.currentkeys;
        // Function is finished
    }
    // Start is called before the first frame update

    public static InputManager FindInstance()
    {
        return FindObjectOfType<InputManager>();
    }

    private void Awake()
    {
        InputManager[] managers = FindObjectsOfType<InputManager>();

        //Check if there is more managers
        if (managers != null && managers.Length > 1)
        {
            // There should never be more than a single App container in the context of this sample.
            Destroy(gameObject);
            return;

        }
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        manageData = new ManageData();
        manageData.LoadData();
        StartCoroutine(LoadInitialKeys());    
    }

    // Update is called once per frame
    /// <summary>
    /// Detects which key has been polished
    /// </summary>
    /// <param name="buttonName"></param>
    /// <returns></returns>
    public bool GetButtonDown(string buttonName)
    {
        if (buttonKeys.ContainsKey(buttonName) == false)
        {
            Debug.Log("InputManager::GetButtonDown -- No button named: " + buttonName);
            return false;
        }
        return Input.GetKeyDown(buttonKeys[buttonName]);
    }
    /// <summary>
    /// Get Names of Keys
    /// </summary>
    /// <returns></returns>
    public string[] GetButtonNames()
    {
        return buttonKeys.Keys.ToArray();
    }
    /// <summary>
    /// Get Name from the button
    /// </summary>
    /// <param name="buttonName"></param>
    /// <returns></returns>
    public string GetKeyNameForButton(string buttonName)
    {
        if (buttonKeys.ContainsKey(buttonName) == false)
        {
            Debug.Log("InputManager::GetButtonDown -- No button named: " + buttonName);
            return "N/A";
        }
        return buttonKeys[buttonName].ToString();
    }
    /// <summary>
    /// Change the Key
    /// </summary>
    /// <param name="buttonName"></param>
    /// <param name="keyCode"></param>
    public void SetButtonForKey(string buttonName, KeyCode keyCode)
    {
        buttonKeys[buttonName] = keyCode;
    }
    public void OnreturnLobbyInput()
    {
        if (SceneManager.GetActiveScene().name != "Lobby")
        {
            SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
            Destroy(this.gameObject);
        }
    }
}
