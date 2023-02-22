using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MoveTabs : MonoBehaviour
{
    public GameObject TabPanel;
    public GameObject TabPanelKeys;
    public GameObject TabPanelPlayers;
    public GameObject TabTabPlayers;
    // Start is called before the first frame update
    void Start()
    {
         if (PhotonNetwork.IsMasterClient)
        {
          TabTabPlayers.SetActive(true);
        }
        TabPanel.SetActive(true);
        TabPanelKeys.SetActive(false);
        TabPanelPlayers.SetActive(false);
    }
    public void ChangeToPanel(){
        TabPanel.SetActive(true);
        TabPanelKeys.SetActive(false);
        TabPanelPlayers.SetActive(false);
    }

    public void ChangeToPanelKeys(){
        TabPanel.SetActive(false);
        TabPanelKeys.SetActive(true);
        TabPanelPlayers.SetActive(false);
    }
    public void ChangeToPanelPlayer(){
        TabPanel.SetActive(false);
        TabPanelKeys.SetActive(false);
        TabPanelPlayers.SetActive(true);
    }

    
 
}
