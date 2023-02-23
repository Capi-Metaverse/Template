using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class MoveTabs : MonoBehaviour , IInRoomCallbacks
{
    public GameObject TabPanel;
    public GameObject TabPanelKeys;
    public GameObject TabPanelPlayers;
    public GameObject TabTabPlayers;
    public GameObject Lenguages;
    public string[] PlayerKeys;
    
   
    // Start is called before the first frame update
    void Start()
    {
         if (PhotonNetwork.IsMasterClient)
        {
          TabTabPlayers.SetActive(true);
        } 
        TabPanel.SetActive(true);
        Lenguages.SetActive(true);
        TabPanelKeys.SetActive(false);
        TabPanelPlayers.SetActive(false);
    }
    public void ChangeToPanel()
    {
        TabPanel.SetActive(true);
        Lenguages.SetActive(false);
        TabPanelKeys.SetActive(false);
        TabPanelPlayers.SetActive(false);
    }

    public void ChangeToPanelKeys()
    {
        TabPanel.SetActive(false);
        Lenguages.SetActive(false);
        TabPanelKeys.SetActive(true);
        TabPanelPlayers.SetActive(false);
    }
    public void ChangeToPanelPlayer()
    {
        TabPanel.SetActive(false);
        Lenguages.SetActive(false);
        TabPanelKeys.SetActive(false);
        TabPanelPlayers.SetActive(true);
    }



    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        throw new System.NotImplementedException();
    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        throw new System.NotImplementedException();
    }

    public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        throw new System.NotImplementedException();
    }

    public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        throw new System.NotImplementedException();
    }

    void IInRoomCallbacks.OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("Cambiosssss");
         if (PhotonNetwork.IsMasterClient)
        {
          TabTabPlayers.SetActive(true);
        } 
        //throw new System.NotImplementedException();
    }
}

