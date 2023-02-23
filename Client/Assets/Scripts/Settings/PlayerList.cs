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


Dictionary <string, int> PlayerToActor;
    

     private void Start()
        {
            //Kickbutton.onClick.AddListener();
        }
    public void listadoPlayer()
    {
        foreach(Transform child in PlayerListSettings.transform)
            {
            Destroy(child.gameObject);
            }
        Player[] otherPlayers = PhotonNetwork.PlayerListOthers;
        string[] PlayerKeys = new string[otherPlayers.Length];
            for (int i = 0; i < otherPlayers.Length; i++)
            {
                PlayerKeys[i] = otherPlayers[i].NickName.ToString();

                Debug.Log(otherPlayers[i].NickName.ToString());
            }
             for(int i = 0; i<PlayerKeys.Length; i++)
            {    // TODO: I need to show a popup message saying the master client left to the other clients
            string bn;
            bn = PlayerKeys[i];

            GameObject go = (GameObject)Instantiate(PlayerItemPrefabSettings);
            go.GetComponent<UserListitem>().numActor = otherPlayers[i].ActorNumber;
            go.transform.SetParent( PlayerListSettings.transform);
            go.transform.localScale = Vector3.one;

            TMP_Text PlayerNameText = go.transform.GetChild(0).GetComponent<TMP_Text>();
            PlayerNameText.text = bn;
            }
    }
}
