using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;

public class NetworkPlayer : NetworkBehaviour,IPlayerLeft
{
    private GameManager gameManager;

    public static NetworkPlayer Local { get; set; }
    [Networked]
    public int avatar { get; set; }
    public GameObject[] playerPrefabs;
    public Animator animator;
    //UI NAME IN GAME
    public TextMeshProUGUI playerNicknameTM;
    [Networked]
    public string playfabIdentity { get; set; }
    [Networked(OnChanged = nameof(OnNickNameChanged))]
    public NetworkString<_16> nickname { get; set; }
    public NetworkString<_16> playfabId { get; set; }
    // Start is called before the first frame update
    [Networked] public int ActorID { get; set; }

    public CharacterInputHandler inputHandler;



    public override void Spawned()
    {
      
        var controller = Resources.Load("Animations/Character") as RuntimeAnimatorController;

        
        //Add animator
        if (this.avatar == 0) this.avatar = Random.Range(1, 6);
        Debug.Log(this.avatar);
        gameManager = GameManager.FindInstance();
        
        GameObject model = Instantiate(playerPrefabs[this.avatar], gameObject.transform.position, gameObject.transform.rotation, gameObject.transform);
        model.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        model.transform.SetAsFirstSibling();
        model.AddComponent<Animator>();
        model.GetComponent<Animator>().runtimeAnimatorController = controller;
        this.gameObject.tag = "Player";

        if (Object.HasInputAuthority)
        {
            gameManager.SetAvatarNumber(avatar);
            playfabIdentity = gameManager.GetUserID();
            Local = this;
            this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            this.inputHandler.enabled = true;
            //Looks in scene for the GameManager and store it
            SetGameManager(GameObject.Find("/Manager").GetComponent<GameManager>());
            gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("LocalPlayer");
            foreach (Transform child in gameObject.transform.GetChild(0))
            {
                child.gameObject.layer = LayerMask.NameToLayer("LocalPlayer");
            
            }
            inputHandler.FindUIOtherUser();
        }

        else Debug.Log("Spawned remote player");
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority) Runner.Despawn(Object);
    }

    //Detects and activates [Networked(OnChanged = nameof(OnNickNameChanged))].
    static void OnNickNameChanged(Changed<NetworkPlayer> changed)
    {
        Debug.Log($"{Time.time} OnHPChanged value {changed.Behaviour.nickname}");

        changed.Behaviour.OnNickNameChanged();
    }

    //Here we put the name to our local player, we don´t need to do more because our nerworked nickname is also setted so since is networked nickname will be load in every client 
    private void OnNickNameChanged()
    {
        Debug.Log($"Nick name changed for player to {nickname} for player {gameObject.name}");
        
        playerNicknameTM.text = nickname.ToString();
        

    }

    //Setter for GameManager
    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

}
