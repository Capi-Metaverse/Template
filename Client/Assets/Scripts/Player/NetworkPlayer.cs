using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;

public class NetworkPlayer : NetworkBehaviour,IPlayerLeft
{
    public static NetworkPlayer Local { get; set; }
    [Networked]
    public int avatar { get; set; }
    public GameObject[] playerPrefabs;
    public Animator animator;
    //UI NAME IN GAME
    public TextMeshProUGUI playerNicknameTM;
    [Networked(OnChanged = nameof(OnNickNameChanged))]
    public NetworkString<_16> nickname { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Spawned()
    {

        var controller = Resources.Load("Animations/Character") as RuntimeAnimatorController;

        //Add animator
        if (this.avatar == 0) this.avatar = Random.Range(1, 6);
        GameObject model = Instantiate(playerPrefabs[this.avatar], gameObject.transform.position, gameObject.transform.rotation, gameObject.transform);
        model.transform.SetAsFirstSibling();
        model.AddComponent<Animator>();
        model.GetComponent<Animator>().runtimeAnimatorController = controller;
        this.gameObject.tag = "Player";

        if (Object.HasInputAuthority)
        {
            Local = this;
            this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            Debug.Log("Spawned local player");

            RPC_SetNickName("NombrePrueba");

        }
        else Debug.Log("Spawned remote player");
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority) Runner.Despawn(Object);
    }

    static void OnNickNameChanged(Changed<NetworkPlayer> changed)
    {
        Debug.Log($"{Time.time} OnHPChanged value {changed.Behaviour.nickname}");

        changed.Behaviour.OnNickNameChanged();
    }

    private void OnNickNameChanged()
    {
        Debug.Log($"Nick name changed for player to {nickname} for player {gameObject.name}");

        playerNicknameTM.text = nickname.ToString();
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_SetNickName(string nickname, RpcInfo info = default)
    {
        Debug.Log($"[RPC]: setNickname {nickname} ");
        this.nickname = nickname;
       
    }

}
