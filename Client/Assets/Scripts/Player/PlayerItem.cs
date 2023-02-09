using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
public class PlayerItem : MonoBehaviourPunCallbacks
{
    /*------------------VARIABLES----------------*/
    public TMP_Text playerName;
    public Color highlightColor;

    public GameObject leftArrowButton;
    public GameObject rightArrowButton;

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    public Image playerAvatar;
    public Sprite[] avatars;

    Player player;

    /*------------------METHODS----------------*/
    private void Start()
    {
        //Set array properties to avatar 6 which es the random character
        playerProperties["playerAvatar"] = 6;
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void SetPlayerInfo(Player _player)
   //When enter to lobby register your name
    {
        playerName.text = _player.NickName;
        player = _player;
        UpdatePlayerItem(player);//Update the list of players in the room
    }

    public Player GetPlayerInfo(){
        return player;
    }

    //This is only executed for the current player, activates the arrow for player selection
    public void ApplyLocalChanges()
    {
        //enable arrows, you´ll only see them on your player preview the other player previews will not have arrows for you
        leftArrowButton.SetActive(true);
        rightArrowButton.SetActive(true);
    }
    public void OnClickLeftArrow()
    {
        //Change Sprite and Avatar to Left
        if((int)playerProperties["playerAvatar"] == 0)
        {
            playerProperties["playerAvatar"] = avatars.Length -1;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] -1;
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void OnClickRightArrow()
    {
        //Change Sprite and Avatar to Right
        if((int)playerProperties["playerAvatar"] == avatars.Length -1)
        {
            playerProperties["playerAvatar"] = 0;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] + 1;
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }
    //When some player change its character this aply it to the view
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable playerProperties)
    {
        if (player == targetPlayer)
            {
                UpdatePlayerItem(targetPlayer);
            }
    }

    void UpdatePlayerItem(Player player)
    {   
        //Update sprite and properties post you change with the arrows your player
        if (player.CustomProperties.ContainsKey("playerAvatar"))
        {
            playerAvatar.sprite = avatars[(int)player.CustomProperties["playerAvatar"]];
            playerProperties["PlayerAvatar"] = (int)player.CustomProperties["playerAvatar"];
        }
        else
        {
            playerProperties["playerAvatar"] = 6;
        }
    }
} 
