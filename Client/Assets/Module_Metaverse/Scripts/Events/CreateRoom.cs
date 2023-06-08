using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour
{

    [SerializeField] private TMP_InputField inputNameCreate;

    [SerializeField] private TMP_InputField inputNameJoin;

    [SerializeField] private TMP_InputField inputPlayers;

    [SerializeField] private TMP_Text map;

    private string[] mapNames = { "Mapa1", "Mapa2", "Oficinas" };

    private int actualMap = 0;


    //Buttons

    [SerializeField] private Button leftPlayers;
    [SerializeField] private Button rightPlayers;

    [SerializeField] private Button leftMap;
    [SerializeField] private Button rightMap;

    //Panels

    [SerializeField] private GameObject choosePanel;
    [SerializeField] private GameObject joinPanel;
    [SerializeField] private GameObject createPanel;




    GameManager gameManager;

    private void Start()
    {
         map.text = mapNames[actualMap];
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
    /// Join in a room whit the name of the input
    /// </summary>
    public void JoinNewRoom()
    {
        if (!inputNameJoin.text.Equals(""))
        {
            Debug.Log("Uniendose a una nueva sala");
            gameManager.JoinCustomGame(inputNameJoin.text);
        }
    }


    public void OpenUI()
    {
        this.gameObject.SetActive(true);
        choosePanel.SetActive(true);
        createPanel.SetActive(false);
        joinPanel.SetActive(false);
        gameManager = GameManager.FindInstance();
        GameObject player = gameManager.GetCurrentPlayer();
        player.GetComponent<CharacterInputHandler>().changeRoomPanel = this.gameObject;
        player.GetComponent<CharacterInputHandler>().DeactivateALL();

    }

    public void CloseUI()
    {
        this.gameObject.SetActive(false);
        GameObject player = gameManager.GetCurrentPlayer();
        player.GetComponent<CharacterInputHandler>().ActiveALL();
    }

    public void OpenJoinUI()
    {
         choosePanel.SetActive(false);
        joinPanel.SetActive(true);
    }

    public void OpenCreateUI()
    {
        choosePanel.SetActive(false);
        createPanel.SetActive(true);
    }

    /// <summary>
    /// change the number of users can enter in the room
    /// </summary>
    public void OnClickLeftPlayer()
    {
        int userCount = Int32.Parse(inputPlayers.text);

        if(userCount > 0)
        {
            inputPlayers.text = (userCount - 1).ToString();
        }
    }

    public void OnClickRightPlayer()
    {
        int userCount = Int32.Parse(inputPlayers.text);

        if (userCount < 8)
        {
            inputPlayers.text = (userCount + 1).ToString();
        }
    }
    /// <summary>
    /// Change map
    /// </summary>
    public void OnClickLeftMap()
    {
        if (actualMap > 0)
        {

            map.text = mapNames[--actualMap];
        }
    }

    public void OnClickRightMap()
    {
        if (actualMap < mapNames.Length - 1 )
        {
           map.text = mapNames[++actualMap];
        }
    }
}
