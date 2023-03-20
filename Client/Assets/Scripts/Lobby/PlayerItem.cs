
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Fusion;


public class PlayerItem : NetworkBehaviour, ISpawned
{

    //WIP
    /*------------------VARIABLES----------------*/

    public TMP_Text playerName;
    public Color highlightColor;

    public GameObject leftArrowButton;
    public GameObject rightArrowButton;
    [Networked]
    public NetworkObject networkObject { get; set; }



    public Image playerAvatar;

    [Networked]
    [SerializeField] private int playerAvatarNumber { get; set; }

    [Networked]
    private string username { get; set; }

    public Sprite[] avatars;

    [Networked]
    PlayerRef player { get; set; }

    GameManager gameManager;

   

    /*------------------METHODS----------------*/
    
    private void Start()
    {
        //Set array properties to avatar 6 which es the random character
        playerAvatarNumber = 0;

    }

    public void Spawn(NetworkRunner runner, PlayerItem player)
    {

        runner.Spawn(
        player,
        Vector3.zero,
        Quaternion.identity,
        inputAuthority: runner.LocalPlayer,
        BeforeSpawn,
        predictionKey: null
        );
    }

    private void BeforeSpawn(NetworkRunner runner, NetworkObject obj)
    {
        gameManager = GameManager.FindInstance();
        PlayerItem item = obj.GetComponent<PlayerItem>();

        Debug.Log("user");
        item.username = gameManager.username;
        item.playerName.text = gameManager.username;
        Debug.Log("ID:" + runner.LocalPlayer);
        item.SetPlayerInfo(runner.LocalPlayer);
        item.networkObject = obj;

        item.leftArrowButton.SetActive(true);
        item.rightArrowButton.SetActive(true);
    

    }

    public override void Spawned()
    {
        base.Spawned();

        //Transform.SetParent();

        Debug.Log(this.GetPlayerInfo().PlayerId);
        transform.SetParent(GameObject.Find("PlayerListener").transform);

        playerName.text = this.username;

        if (Object.HasInputAuthority)
        {
            gameManager = GameManager.FindInstance();
           
           
            Rpc_SetNickname(gameManager.username);
        }

        playerAvatar.sprite = avatars[playerAvatarNumber];

        LobbyManager _lobbyManager = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
     
        


    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    void Rpc_SetNickname(string nick)
    {
        Debug.Log("Cambiando");
        Debug.Log(nick);
        username = nick;
        playerName.text = nick;
    }






    public void SetPlayerInfo(PlayerRef _player)
    //When enter to lobby register your name
    {
     
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

        playerAvatar.sprite = avatars[playerAvatarNumber];
        //Dont know if you have to send a RPC
    }

    public void OnClickRightArrow()
    {
        //Change Sprite and Avatar to Left
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
        Rpc_SetImage(playerAvatarNumber);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    void Rpc_SetImage(int playerNumber)
    {

        playerAvatar.sprite = avatars[playerAvatarNumber];
 
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