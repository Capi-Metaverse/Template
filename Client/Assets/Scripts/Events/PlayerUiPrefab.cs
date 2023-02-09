using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerUiPrefab : MonoBehaviour
{
    public TMP_Text pressKey;
    string eventText;
    string change;
    // Start is called before the first frame update
    void Start()
    {
        eventText = GameObject.Find("PlayerUIPrefab").transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text;

        eventText = "Press E to interact";
        pressKey.text = eventText;
        Debug.Log(eventText);
        
    }
    
    public void ChangeLetter(string change)
    {
        eventText = $"Press {change} to interact";
        Debug.Log(eventText);
        pressKey.text = eventText;
    }
   
}
