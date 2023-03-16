using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Fusion;

public class PlayerItem : NetworkBehaviour
{
    /*------------------VARIABLES----------------*/
    public TMP_Text playerName;
    public Color highlightColor;

    public GameObject leftArrowButton;
    public GameObject rightArrowButton;

    
    public Image playerAvatar;

    [Networked]
    private int playerAvatarNumber { get; set; }

    [Networked]
    private string username { get; set; }

    public Sprite[] avatars;

    PlayerRef player;

    GameManager gameManager;

    /*------------------METHODS----------------*/
    private void Awake()
    {
        //We got the game manager
        gameManager = GameManager.FindInstance();
        username = gameManager.username;
    }
    private void Start()
    {
        //Set array properties to avatar 6 which es the random character
        playerAvatarNumber = 6;
       
    }

    public void SetPlayerInfo(PlayerRef _player)
    //When enter to lobby register your name
    {
        playerName.text = username;
        player = _player;
        //UpdatePlayerItem(player);//Update the list of players in the room
    }

    public PlayerRef GetPlayerInfo()
    {
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
        if ( playerAvatarNumber == 0)
        {
            playerAvatarNumber = avatars.Length - 1;
        }
        else
        {
            playerAvatarNumber = playerAvatarNumber - 1;
        }
        //Dont know if you have to send a RPC
    }

    public void OnClickRightArrow()
    {
        //Change Sprite and Avatar to Left
        if (playerAvatarNumber == 0)
        {
            playerAvatarNumber = avatars.Length - 1;
        }
        else
        {
            playerAvatarNumber = playerAvatarNumber - 1;
        }
        //Dont know if you have to send a RPC
    }

    //Treat RPC conn

    /*
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
    */
}