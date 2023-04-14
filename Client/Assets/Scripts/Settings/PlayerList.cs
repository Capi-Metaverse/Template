using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Linq;

public class PlayerList : MonoBehaviour
{
    public GameObject PlayerItemPrefabSettings;
    public GameObject PlayerListSettings;
    List<string> NamePlayer = new List<string>();

    public int IDPlayer;
    private NetworkRunner _runner;
    // Start is called before the first frame update
    //Function to get the List of players and Kick them

    public Dictionary<int, string> PlayerDict;
    public void ListPlayers()
    {
        //Destroys the former list
        foreach (Transform child in PlayerListSettings.transform)
        {
            Destroy(child.gameObject);
        }
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        PlayerDict = new Dictionary<int, string>();
        foreach (GameObject player in players)
        {

            // NamePlayer.Add(player.GetComponent<NetworkPlayer>().nickname.ToString());
            IDPlayer = player.GetComponent<NetworkPlayer>().ActorID;
            Debug.Log(player.GetComponent<NetworkPlayer>().nickname.ToString());

            PlayerDict[IDPlayer] = player.GetComponent<NetworkPlayer>().nickname.ToString();
            //Debug.Log(PlayerDict);
        }



        //Iterate players to get Nickname && ActorNumber
        for (int i = 0; i < PlayerDict.Count; i++)
        {
            //We create the userItem object
            GameObject userItem = (GameObject)Instantiate(PlayerItemPrefabSettings);

            userItem.transform.SetParent(PlayerListSettings.transform);
            userItem.transform.localScale = Vector3.one;





            //We configure the Nickname
            TMP_Text PlayerNameText = userItem.transform.GetChild(0).GetComponent<TMP_Text>();
            PlayerNameText.text = PlayerDict[i];


            List<int> keys = new List<int>(PlayerDict.Keys);
            //We configure the ActorNumber
            userItem.GetComponent<UserListItem>().numActor = keys[i];
            Debug.Log(keys[i]);
        }
    }
}