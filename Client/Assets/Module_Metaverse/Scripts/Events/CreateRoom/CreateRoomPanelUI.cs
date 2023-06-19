using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoomPanelUI : MonoBehaviour
{

    //Panels
    [SerializeField] private GameObject choosePanel;
    [SerializeField] private GameObject joinPanel;
    [SerializeField] private GameObject createPanel;

    GameManager gameManager;

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
}
