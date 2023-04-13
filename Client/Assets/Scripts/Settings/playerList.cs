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
    [Networked] NetworkPlayer networkPlayer { get; set; }

    private NetworkRunner _runner;
    // Start is called before the first frame update
    //Function to get the List of players and Kick them

    
    public void playerList()
    {
        //Destroys the former list
        foreach (Transform child in PlayerListSettings.transform)
        {
            Destroy(child.gameObject);
        }
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players) 
        {
            NamePlayer.Add(player.GetComponent<NetworkPlayer>().nickname.ToString());
            Debug.Log(player.GetComponent<NetworkPlayer>().nickname.ToString());
        }

        Debug.Log("ppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppp");

        //Iterate players to get Nickname && ActorNumber
        /*for (int i = 0; i < a.Length; i++)
        {
            //We create the userItem object
            GameObject userItem = (GameObject)Instantiate(PlayerItemPrefabSettings);

            userItem.transform.SetParent(PlayerListSettings.transform);
            userItem.transform.localScale = Vector3.one;



            //We configure the Nickname
            TMP_Text PlayerNameText = userItem.transform.GetChild(0).GetComponent<TMP_Text>();
            PlayerNameText.text = otherPlayers[i].NickName.ToString();



            //We configure the ActorNumber
            userItem.GetComponent<UserListitem>().numActor = otherPlayers[i].ActorNumber;
        }*/
    }
}