using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class DoorEvent : MonoBehaviour, IMetaEvent
{

    public string map;

    
   [SerializeField] private bool isPublic = true;
   [SerializeField] private string password;
    
   [SerializeField] private PasswordScript passwordScript;
    public string lastPassword = "";
    GameManager gameManager;

    public void activate(bool host)
    {

        //If the room is public, we change the map
        if (isPublic)
        {

            gameManager = GameManager.FindInstance();

            //Activate the loading UI

            //GameObject Loading = GameObject.Find("PlayerUIPrefab").transform.GetChild(4).gameObject;
            //Loading.SetActive(true);

            gameManager.ChangeMap(map);
        }

        //If the room is not public, we will ask the password

        else
        {

            //We get CharacterInputHandler
            


            //Password
            if (!lastPassword.Equals(password))
            {
                
                passwordScript.OpenUI(this);
            }

            else
            {
                gameManager = GameManager.FindInstance();
                gameManager.ChangeMap(map);
            }
        }

 






    }


}