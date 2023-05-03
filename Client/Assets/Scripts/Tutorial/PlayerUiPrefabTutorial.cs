using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerUiPrefabTutorial : MonoBehaviour
{
    public TMP_Text pressKey;
    public TMP_Text pressKeyK;
    string eventText;
    string eventTextK;

    //private InputManager inputManager;
    // Start is called before the first frame update
    void Start()
    {

        //inputManager = GameManager.FindInstance().GetComponentInChildren<InputManager>();


        //eventText = GameObject.Find("PlayerUIPrefab").transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text;
        //eventTextK = GameObject.Find("PlayerUIPrefab").transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text;

        //ChangeLetter(inputManager.buttonKeys["Interact"].ToString());
        //ChangeLetterK(inputManager.buttonKeys["ChangeCamera"].ToString());

    }

    //public void ChangeLetter(string change)
    //{
    //    eventText = $"Press {change} to interact";
    //    pressKey.text = eventText;
    //}
    //public void ChangeLetterK(string change)
    //{
    //    eventTextK = $"Press {change} to enable presentation mode";
    //    pressKeyK.text = eventTextK;
    //}

}
