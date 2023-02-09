using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerUiPrefab : MonoBehaviour
{
    public TMP_Text pressKey;
    public TMP_Text pressKeyK;
    string eventText;
    string eventTextK;
    string change;
    // Start is called before the first frame update
    void Start()
    {
        eventText = GameObject.Find("PlayerUIPrefab").transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text;
        eventTextK = GameObject.Find("PlayerUIPrefab").transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text;
        //eventText = "Press E to interact";
        //eventTextK = "Press K to enable presentation mode";
        pressKey.text = eventText;
        pressKeyK.text = eventTextK;
        Debug.Log(eventText);
        
    }
    
    public void ChangeLetter(string change)
    {
        eventText = $"Press {change} to interact";
        Debug.Log(eventText);
        pressKey.text = eventText;
    }
    public void ChangeLetterK(string change)
    {
        eventTextK = $"Press {change} to enable presentation mode";
        Debug.Log(eventTextK);
        pressKeyK.text = eventTextK;
    }
   
}
