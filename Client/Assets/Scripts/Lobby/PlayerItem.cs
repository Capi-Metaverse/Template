
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Fusion;
using ExitGames.Client.Photon.StructWrapping;
using static Unity.Collections.Unicode;


public class PlayerItem : NetworkBehaviour, ISpawned
{
    /*------------------VARIABLES----------------*/

    //PlayerName Text
    [SerializeField] private TMP_Text playerName;
 

    //Buttons

    [SerializeField] private GameObject leftArrowButton;
    [SerializeField] private GameObject rightArrowButton;

    //Networked attributes
    [Networked]
    private NetworkObject networkObject { get; set; }

    [Networked]
    [SerializeField] private int playerAvatarNumber { get; set; }

    [Networked]
    private string username { get; set; }

    [Networked]
    private PlayerRef player { get; set; }



    //Avatar Image Related

    [SerializeField] private Image playerAvatar;
    [SerializeField] private Sprite[] avatars;

    //LobbyManager
    [SerializeField] private LobbyManager _lobbyManager;


    /*------------------METHODS----------------*/

    private void Start()
    {
        //Set array properties to avatar 6 which es the random character
        playerAvatarNumber = 0;

    }

    public void setInfo( GameManager gameManager, NetworkRunner runner, NetworkObject obj)
    {

       this._lobbyManager = GameObject.FindObjectOfType<LobbyManager>();

        this.username = gameManager.GetUsername();
        this.playerName.text = gameManager.GetUsername();
        this.player = runner.LocalPlayer;
        this.networkObject = obj;
        this.leftArrowButton.SetActive(true);
        this.rightArrowButton.SetActive(true);
    }

    public override void Spawned()
    {
        base.Spawned();

        Debug.Log("[PlayerItem] ID:" + player + "has spawned a item.");

        transform.SetParent(GameObject.Find("PlayerListener").transform);

        //We set the text && the avatar in all the users.

        playerName.text = this.username;
        playerAvatar.sprite = avatars[playerAvatarNumber];

    }

    //Image Button Left
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

        playerAvatar.sprite = avatars[playerAvatarNumber];
        _lobbyManager.SetAvatarNumber(playerAvatarNumber);
        Rpc_SetImage(playerAvatarNumber);

    }
    //Image Button Right
    public void OnClickRightArrow()
    {
        //Change Sprite and Avatar to Right
        if (playerAvatarNumber == avatars.Length - 1)
        {
            playerAvatarNumber = 0;
        }
        else
        {
            playerAvatarNumber = playerAvatarNumber + 1;
        }
        //Dont know if you have to send a RPC
        playerAvatar.sprite = avatars[playerAvatarNumber];
        _lobbyManager.SetAvatarNumber(playerAvatarNumber);
        Rpc_SetImage(playerAvatarNumber);
    }

    //Function that executes when a user change their image
    [Rpc(RpcSources.All, RpcTargets.All)]
    void Rpc_SetImage(int playerNumber)
    {

        playerAvatar.sprite = avatars[playerAvatarNumber];
 
    }
  
}