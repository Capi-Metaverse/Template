using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreateRoom : MonoBehaviour
{

   [SerializeField] private TMP_InputField inputName;


    GameManager gameManager;
  

    public void CreateNewRoom()
    {

        Debug.Log("Entro");
        Debug.Log(gameManager);
        gameManager.StartGame(inputName.text, gameManager.GetAvatarNumber());
       

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
}
