using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Collections.Unicode;

public class FormManager : MonoBehaviour
{

    [SerializeField] private TMP_InputField email;
    [SerializeField] private TMP_InputField password;

    [SerializeField] private TMP_Text error;

    //Game Manager
    private GameManager gameManager;

    private void Awake()
    {
        //Get the GameManager
        gameManager = GameManager.FindInstance();


    }
    public void onSignUp()
    {
        //Change panel to register

        //SignUp Logic

        //Change to login when ended

        Debug.Log("User Created Succesfully");

    }

    public async void onLogin()
    {
        if (email.text == "" || password.text == "") {

            //We will return an error if the fields are null
            Debug.Log("No inputs");
            return;

        }

        //Petition to playfab

        //Login logic

        //Set username && ID on GameManager

        
        //Gamemanager.setId();

        Debug.Log("User Logged Succesfully");

        
        SceneManager.LoadSceneAsync("Lobby");


    }



   
}
