using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PlayerList : MonoBehaviour
{
public string[] PlayerKeys;
public GameObject PlayerItemPrefabSettings;
public GameObject PlayerListSettings;
    // Update is called once per frame
    public void listadoJugadores()
    {
        Player[] otherPlayers = PhotonNetwork.PlayerListOthers;
            for (int i = 0; i < otherPlayers.Length; ++i)
            {
                PlayerKeys = new string[]{otherPlayers[i].ToString()} ;
                Debug.Log(PlayerKeys);
                // TODO: I need to show a popup message saying the master client left to the other clients
            } 
            for(int i = 0; i<PlayerKeys.Length; i++)
            {
            string bn;
            bn = PlayerKeys[i];

            GameObject go = (GameObject)Instantiate(PlayerItemPrefabSettings);
            go.transform.SetParent( PlayerListSettings.transform);
            go.transform.localScale = Vector3.one;

            TMP_Text PlayerNameText = go.transform.GetChild(0).GetComponent<TMP_Text>();
            PlayerNameText.text = bn;
            }
    }

}
