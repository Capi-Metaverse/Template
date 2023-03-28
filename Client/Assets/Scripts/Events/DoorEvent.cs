using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class DoorEvent : MonoBehaviour, IMetaEvent
{

    public string map;
    GameManager gameManager;

    public void activate(bool host)
    {

        gameManager = GameManager.FindInstance();

        //Activate the loading UI

        //GameObject Loading = GameObject.Find("PlayerUIPrefab").transform.GetChild(4).gameObject;
        //Loading.SetActive(true);

        gameManager.ChangeMap(map);

 






    }


}