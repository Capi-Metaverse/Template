using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerList : MonoBehaviour
{

public GameObject PlayerItemPrefabSettings;
public GameObject PlayerListSettings;

    //Function to get the List of players and Kick them
    public void playerList()
    {
        //Destroys the former list
        foreach(Transform child in PlayerListSettings.transform)
            {
            Destroy(child.gameObject);
            }
        //Get the list of players from Photon.
        Player[] otherPlayers = PhotonNetwork.PlayerListOthers;

        //Iterate players to get Nickname && ActorNumber
            for (int i = 0; i < otherPlayers.Length; i++)
            {
                //We create the userItem object
                GameObject userItem = (GameObject)Instantiate(PlayerItemPrefabSettings);
                
                userItem.transform.SetParent( PlayerListSettings.transform);
                userItem.transform.localScale = Vector3.one;

                //We configure the Nickname
                TMP_Text PlayerNameText = userItem.transform.GetChild(0).GetComponent<TMP_Text>();
                PlayerNameText.text = otherPlayers[i].NickName.ToString();

                //We configure the ActorNumber
                userItem.GetComponent<UserListitem>().numActor = otherPlayers[i].ActorNumber;
            }
    }
}
