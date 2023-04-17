using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour
{

   [SerializeField] private TMP_InputField inputName;

    [SerializeField] private TMP_InputField inputPlayers;

    [SerializeField] private TMP_Text map;

    private string[] mapNames = { "Mapa1", "Mapa2", "Oficinas" };

    private int actualMap = 0;


    //Buttons

    [SerializeField] private Button leftPlayers;
    [SerializeField] private Button rightPlayers;

    [SerializeField] private Button leftMap;
    [SerializeField] private Button rightMap;





    GameManager gameManager;

    private void Start()
    {
         map.text = mapNames[actualMap];
    }


    public void CreateNewRoom()
    {

        Debug.Log("Entro");
        Debug.Log(gameManager);
        gameManager.StartCustomGame(inputName.text, Int32.Parse(inputPlayers.text), map.text);
       

    }


    public void OpenUI()
    {
        this.gameObject.SetActive(true);
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
