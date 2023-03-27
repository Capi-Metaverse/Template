using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class KeyBindDialogueBox : MonoBehaviour
{
    InputManager inputManager;
    public GameObject keyItemPrefab;
    public GameObject keyList;
    string buttonToRebind = null;
    Dictionary<string, TMP_Text> buttonToLabel;
    PlayerUiPrefab playerUiPrefab;
    TMP_Text keyNameText;
    Keys keys;

    ManageData manageData;



    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameObject.FindObjectOfType<InputManager>();
        playerUiPrefab = GameObject.FindObjectOfType<PlayerUiPrefab>();
        //Create one "Key List Item"

        manageData = new ManageData();



        string[] buttonNames = inputManager.GetButtonNames();
        buttonToLabel = new Dictionary<string, TMP_Text>();

        //foreach(string bn in buttonNames)
        for (int i = 0; i < buttonNames.Length; i++)
        {
            string bn;
            bn = buttonNames[i];

            GameObject go = (GameObject)Instantiate(keyItemPrefab);
            go.transform.SetParent(keyList.transform);
            go.transform.localScale = Vector3.one;

            TMP_Text buttonNameText = go.transform.GetChild(0).GetComponent<TMP_Text>();
            buttonNameText.text = bn;


            keyNameText = go.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
            keyNameText.text = inputManager.GetKeyNameForButton(bn);
            buttonToLabel[bn] = keyNameText;

            Button keyBindButton = go.transform.GetChild(1).GetComponent<Button>();
            keyBindButton.onClick.AddListener(() => { StartRebinFor(bn); });
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonToRebind != null)
        {
            if (Input.anyKeyDown)
            {
                //which key was pressed down
                //Loop through all possible keys
                foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(kc))
                    {
                        inputManager.SetButtonForKey(buttonToRebind, kc);
                        buttonToLabel[buttonToRebind].text = kc.ToString();

                        if (buttonToRebind == "Interact")
                        {
                           
                            playerUiPrefab.ChangeLetter(kc.ToString());
                            inputManager.a = ((int)kc);
                            manageData.SaveData(((int)kc), inputManager.b);
                        }

                        if (buttonToRebind == "ChangeCamera")
                        {
                            playerUiPrefab.ChangeLetterK(kc.ToString());
                            inputManager.b = ((int)kc);
                            manageData.SaveData(inputManager.a, ((int)kc));
                        }
                        buttonToRebind = null;
                        break;
                    }
                }
            }
        }
    }
    void StartRebinFor(string buttonName)
    {
        Debug.Log("StartRebinFor: " + buttonName);
        buttonToRebind = buttonName;
    }
}
