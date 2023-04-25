using System.Collections;
using System.Collections.Generic;
//using Photon.Pun;
//using Photon.Realtime;
using TMPro;
using UnityEngine;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

public class MoveTabs : MonoBehaviour //,IInRoomCallbacks
{
    public GameObject TabPanel;
    public GameObject TabPanelKeys;
    public GameObject TabPanelPlayers;
    public GameObject TabPanelFriends;
    public GameObject TabTabPlayers;
    public GameObject Lenguages;
    public string[] PlayerKeys;
    GameObject Settings;
    GameObject Pause;

    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Manager").GetComponent<GameManager>();

        /*
        if (gameManager.GetUserRole() == UserRole.Admin)
        {
          TabTabPlayers.SetActive(true);
        }
        */

      

        TabTabPlayers.SetActive(true);
        TabPanel.SetActive(true);
        TabPanelKeys.SetActive(false);
        Settings = GameObject.Find("Menus").transform.GetChild(0).gameObject;
        Pause = GameObject.Find("Menus").transform.GetChild(1).gameObject;
        TabPanelPlayers.SetActive(false);
    }
    public void ChangeToPanel()
    {
        TabPanel.SetActive(true);
        TabPanelKeys.SetActive(false);
        TabPanelPlayers.SetActive(false);
        TabPanelFriends.SetActive(false);
    }

    public void ChangeToPanelKeys()
    {
        TabPanel.SetActive(false);
      
        TabPanelKeys.SetActive(true);
        TabPanelPlayers.SetActive(false);
        TabPanelFriends.SetActive(false);
    }
    public void ChangeToPanelPlayer()
    {

        //We initialize the player list
        PlayerList playerList = GameObject.Find("TabPlayer").GetComponent<PlayerList>();
        playerList.ListPlayers();

        TabPanel.SetActive(false);
      
        TabPanelKeys.SetActive(false);
        TabPanelPlayers.SetActive(true);
        TabPanelFriends.SetActive(false);
    }
    public void ChangeToPanelFriends()
    {
        

        
        TabPanel.SetActive(false);
       
        TabPanelKeys.SetActive(false);
        TabPanelPlayers.SetActive(false);
        TabPanelFriends.SetActive(true);
        
    }

    public void OnClickBackToPause()
    {
        Pause.SetActive(true);
        Settings.SetActive(false);
    }

}