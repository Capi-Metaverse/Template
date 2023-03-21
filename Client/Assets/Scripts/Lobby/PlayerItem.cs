
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Fusion;


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

    /*------------------METHODS----------------*/
    
    private void Start()
    {
        //Set array properties to avatar 6 which es the random character
        playerAvatarNumber = 0;

    }

    public static void Spawn(NetworkRunner runner, PlayerItem player)
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

    //Function that initializes the object on the first user
    public static void BeforeSpawn(NetworkRunner runner, NetworkObject obj)
    {
        //We get the GameManager for username
        GameManager gameManager = GameManager.FindInstance();

        //We get the network object
        PlayerItem item = obj.GetComponent<PlayerItem>();

     
        item.username = gameManager.username;
        item.playerName.text = gameManager.username;
        item.player = runner.LocalPlayer;
        item.networkObject = obj;
        item.leftArrowButton.SetActive(true);
        item.rightArrowButton.SetActive(true);

    }

    public override void Spawned()
    {
        base.Spawned();

        Debug.Log("ID:" + player + "has spawned a item.");

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
        Rpc_SetImage(playerAvatarNumber);
    }

    //Function that executes when a user change their image
    [Rpc(RpcSources.All, RpcTargets.All)]
    void Rpc_SetImage(int playerNumber)
    {

        playerAvatar.sprite = avatars[playerAvatarNumber];
 
    }
  
}