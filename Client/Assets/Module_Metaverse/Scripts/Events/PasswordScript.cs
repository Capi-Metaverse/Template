using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PasswordScript : MonoBehaviour
{

    //Script of the Password Menu

    //DoorScript
    private DoorEvent doorEvent;

    //Password Input
    [SerializeField]
    private TMP_InputField inputField;

    //Admin change password button
    [SerializeField]
    private Button changeButton;

    //Text titles
    [SerializeField]
    private TMP_Text passwordTitle;
    [SerializeField]
    private TMP_Text passwordChangeTitle;

    //Buttons
    [SerializeField]
    private Button submitPassword;
    [SerializeField]
    private Button submitPasswordChange;

    //Mode
    private bool mode = true;

    //Method to open the Password Menu UI
    public void OpenUI(DoorEvent doorEvent)
    {
        GameManager gameManager = GameManager.FindInstance();
        UserManager userManager = UserManager.FindInstance();
        GameObject player = PhotonManager.FindInstance().CurrentPlayer;

        UIManager uiManager = UIManager.FindInstance();
        PauseManager pauseManager = PauseManager.FindInstance();

        pauseManager.Pause();
        uiManager.SetUIOff();


        // player.GetComponent<CharacterInputHandler>().DeactivateALL();

        this.gameObject.SetActive(true);

        //If this user is admin, activate changePasswordbutton
        if(userManager.UserRole == UserRole.Admin) changeButton.gameObject.SetActive(true);



        this.doorEvent = doorEvent;
    }

    //Method to submit the form
    public void Submit()
    {
        //Change the last password written and calls the function
        doorEvent.lastPassword = inputField.text;
        doorEvent.activate(true);

    }

    //Method to close the menu
    public void Close()
    {
        GameObject player = PhotonManager.FindInstance().CurrentPlayer;
        this.gameObject.SetActive(false);
        //player.GetComponent<CharacterInputHandler>().ActiveALL();
    }

    public void ChangeLayout()
    {
        if (mode)
        {



            //Activate change password layout
            passwordChangeTitle.gameObject.SetActive(true);
            submitPasswordChange.gameObject.SetActive(true);

            //Deactivate original layout
            passwordTitle.gameObject.SetActive(false);
            submitPassword.gameObject.SetActive(false);

         

        }

        else {

            //Activate original layout
            passwordTitle.gameObject.SetActive(true);
            submitPassword.gameObject.SetActive(true);

            //Deactivate change password layout
            passwordChangeTitle.gameObject.SetActive(false);
            submitPasswordChange.gameObject.SetActive(false);

           



        }
        mode = !mode;

    }

    public void ChangePassword()
    {
        doorEvent.setPassword(inputField.text);
    }
}
