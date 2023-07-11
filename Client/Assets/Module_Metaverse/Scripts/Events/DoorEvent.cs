using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Fusion;

namespace Event
{
    public class DoorEvent : MonoBehaviour, IMetaEvent
    {

        public string map;

        GameObject _eventObject;
        GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }
        [SerializeField] private bool isPublic = true;


        [SerializeField] private string password { get; set; }

        [SerializeField] private PasswordScript passwordScript;
        public string lastPassword = "";
        PhotonManager photonManager;

        public void activate(bool host)
        {
            photonManager = PhotonManager.FindInstance();
            UserManager userManager = UserManager.FindInstance();
            //If the room is public, we change the map
            if (isPublic || userManager.UserRole == UserRole.Admin)
            {
                //Activate the loading UI
                photonManager.ChangeScene(map);
            }
            //If the room is not public, we will ask the password
            else
            {
                //Password
                if (!lastPassword.Equals(password))
                {

                    passwordScript.OpenUI(this);
                }

                else
                {
                    photonManager = PhotonManager.FindInstance();
                    photonManager.ChangeScene(map);
                }
            }
        }

        public void setPassword(string password)
        {
            this.password = password;
        }


    }
}
