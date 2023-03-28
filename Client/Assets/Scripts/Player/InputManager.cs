using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public Dictionary<string, KeyCode> buttonKeys;
    public ManageData manageData;

    public Keys currentKeys;

    IEnumerator LoadInitialKeys()
    {
        // Do some work here...
        yield return null; // Wait one frame

        // Do some more work here...
        yield return new WaitForSeconds(4); // Wait for 2 seconds
        
        buttonKeys = new Dictionary<string, KeyCode>();
        if (manageData.currentkeys == null)
        {
            buttonKeys["Interact"] = KeyCode.E;
            buttonKeys["ChangeCamera"] = KeyCode.K;

            manageData.currentkeys = new Keys((int)KeyCode.E, (int)KeyCode.K);
            manageData.SaveData(manageData.currentkeys);
        }
        else
        {
            Debug.Log("entro al else");
            buttonKeys["Interact"] = (KeyCode)manageData.currentkeys.interact;
            buttonKeys["ChangeCamera"] = (KeyCode)manageData.currentkeys.presentationMode;

        }
        currentKeys = manageData.currentkeys;
        // Function is finished
    }
    // Start is called before the first frame update
    void Start()
    {
        manageData = new ManageData();
       
        manageData.LoadData();
        DontDestroyOnLoad(this.gameObject);
        StartCoroutine(LoadInitialKeys());
       
    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool GetButtonDown(string buttonName)
    {
        if (buttonKeys.ContainsKey(buttonName) == false)
        {
            Debug.Log("InputManager::GetButtonDown -- No button named: " + buttonName);
            return false;
        }
        return Input.GetKeyDown(buttonKeys[buttonName]);
    }

    public string[] GetButtonNames()
    {
        return buttonKeys.Keys.ToArray();
    }

    public string GetKeyNameForButton(string buttonName)
    {
        if (buttonKeys.ContainsKey(buttonName) == false)
        {
            Debug.Log("InputManager::GetButtonDown -- No button named: " + buttonName);
            return "N/A";
        }
        return buttonKeys[buttonName].ToString();
    }
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
