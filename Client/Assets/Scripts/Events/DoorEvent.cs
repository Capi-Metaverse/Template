using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Fusion;

public class DoorEvent : MonoBehaviour, IMetaEvent
{

    public string map;

    GameObject _eventObject;
    GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }
    [SerializeField] private bool isPublic = true;

  
   [SerializeField] private string password {  get; set; }
    
   [SerializeField] private PasswordScript passwordScript;
    public string lastPassword = "";
    GameManager gameManager;

    public void activate(bool host)
    {
        gameManager = GameManager.FindInstance();

        //If the room is public, we change the map
        if (isPublic || gameManager.GetUserRole() == UserRole.Admin )
        {

            //Activate the loading UI

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

    public void setPassword(string password)
    {
        this.password = password;
    }


}