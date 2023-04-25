using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Linq;
using Unity.Entities.UniversalDelegates;

public class PlayerList : MonoBehaviour
{
    public GameObject PlayerItemPrefabSettings;
    public GameObject PlayerListSettings;

    public void ListPlayers()
    {
        //Destroys the former list
        foreach (Transform child in PlayerListSettings.transform)
        {
            Destroy(child.gameObject);
        }
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        //We get the player list
        foreach (GameObject player in players)
        {
            //We get the network player

            NetworkPlayer networkPlayer = player.GetComponent<NetworkPlayer>();


            //We create the userItem object
            GameObject userItem = (GameObject)Instantiate(PlayerItemPrefabSettings);

            userItem.transform.SetParent(PlayerListSettings.transform);
            userItem.transform.localScale = Vector3.one;

            //We configure the Nickname
            TMP_Text PlayerNameText = userItem.transform.GetChild(0).GetComponent<TMP_Text>();
            PlayerNameText.text = networkPlayer.nickname.ToString();

            //We get the userItem component

            userItem.GetComponent<UserListItem>().NumActor = networkPlayer.ActorID;



        }

    }
}