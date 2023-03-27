using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkPlayer : NetworkBehaviour,IPlayerLeft
{
    public static NetworkPlayer Local { get; set; }
    public int avatar;
    public GameObject[] playerPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Spawned()
    {

        if (avatar == 0) avatar = Random.Range(1, 6);
        GameObject model = Instantiate(playerPrefabs[avatar], gameObject.transform.position, Quaternion.identity, gameObject.transform);
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
