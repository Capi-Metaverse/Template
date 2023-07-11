using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class InputManager : MonoBehaviour
    {
        public Dictionary<string, KeyCode> buttonKeys;
        public ManageData manageData;

        public Keys currentKeys;

        /// <summary>
        /// Detect if playfab has default values and if not, create default values, and update them in playfab.
        /// </summary>
        /// <returns></returns>
        IEnumerator LoadInitialKeys()
        {
            yield return null; // Wait one frame
            yield return new WaitForSeconds(4); // Wait for 4 seconds

            buttonKeys = new Dictionary<string, KeyCode>();

            //IF no data is saved on playfab for the keys, THEN set buttonKeys & manageData.currentKeys to default values and upload it to playfab
            if (manageData.currentkeys == null)
            {
                buttonKeys["Interact"] = KeyCode.E;
                buttonKeys["ChangeCamera"] = KeyCode.K;
                buttonKeys["Wheel"] = KeyCode.B;
                buttonKeys["OpenMiniMap"] = KeyCode.C;
                buttonKeys["MuteVoice"] = KeyCode.M;

                manageData.currentkeys = new Keys((int)KeyCode.E, (int)KeyCode.K, (int)KeyCode.B, (int)KeyCode.C, (int)KeyCode.M);
                manageData.SaveCurrentKeysDataPlayfab(manageData.currentkeys);
            }

            //IF there is keys data on playfab, THEN set buttonKeys with that info
            else
            {
                //Assign key values from PlayFab
                buttonKeys["Interact"] = (KeyCode)manageData.currentkeys.interact;
                buttonKeys["ChangeCamera"] = (KeyCode)manageData.currentkeys.presentationMode;
                buttonKeys["Wheel"] = (KeyCode)manageData.currentkeys.wheel;
                buttonKeys["OpenMiniMap"] = (KeyCode)manageData.currentkeys.openMiniMap;
                buttonKeys["MuteVoice"] = (KeyCode)manageData.currentkeys.muteVoice;

            }

            //Now manageData.currentkeys is filled with data so we set currentKeys
            currentKeys = manageData.currentkeys;
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

        /// <summary>
        /// At start instanciate manageData to (GetKeysDataFromPlayfab) then Load the retrived keys into class fields (LoadInitialKeys)
        /// </summary>
        void Start()
        {
            manageData = new ManageData();
            manageData.GetCurrentKeysDataPlayfab();
            StartCoroutine(LoadInitialKeys());
        }

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
        /// Change the KeyCode of a determined action on the UI button
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
}
