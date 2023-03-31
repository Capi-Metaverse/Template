using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkPlayer : NetworkBehaviour,IPlayerLeft
{
    public static NetworkPlayer Local { get; set; }
    [Networked]
    public int avatar { get; set; }
    public GameObject[] playerPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Spawned()
    {

        if (this.avatar == 0) this.avatar = Random.Range(1, 6);
        GameObject model = Instantiate(playerPrefabs[this.avatar], gameObject.transform.position, gameObject.transform.rotation, gameObject.transform);
        model.transform.SetAsFirstSibling();

        if (Object.HasInputAuthority)
        {
            Local = this;
            this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            Debug.Log("Spawned local player");

        }
        else Debug.Log("Spawned remote player");
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority) Runner.Despawn(Object);
    }

}
