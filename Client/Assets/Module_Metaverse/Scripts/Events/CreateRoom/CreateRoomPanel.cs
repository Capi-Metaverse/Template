using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomPanel : MonoBehaviour
{

    [SerializeField] private TMP_InputField inputNameCreate;

    [SerializeField] private TMP_InputField inputNameJoin;

    [SerializeField] private TMP_InputField inputPlayers;

    [SerializeField] private TMP_Text map;

    private string[] mapNames = { "Mapa1", "Mapa2", "Oficinas","Map_Photon_Module" };


    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.FindInstance();
        map.text = mapNames[0];
    }

    /// <summary>
    /// Check that the inputs are not empty and create a new room with the value of the inputs.
    /// </summary>
    public void CreateNewRoom()
    {

        if (!inputNameCreate.text.Equals("") && !inputPlayers.text.Equals(""))
        {
            Debug.Log("Creando a una nueva sala");
            gameManager.StartCustomGame(inputNameCreate.text, Int32.Parse(inputPlayers.text), map.text);
        }
        //Else mensaje de error

    }
    /// <summary>
    /// Join a room whith the name of the input
    /// </summary>
    public void JoinNewRoom()
    {
        if (!inputNameJoin.text.Equals(""))
        {
            Debug.Log("Uniendose a una nueva sala");
            gameManager.JoinCustomGame(inputNameJoin.text);
        }
    }
}
