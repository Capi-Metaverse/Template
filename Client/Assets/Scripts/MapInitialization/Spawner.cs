using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine.Diagnostics;
using Photon.Realtime;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkPlayer playerPrefab;

    //Other components
    CharacterInputHandler characterInputHandler;

    GameManager gameManager;
    public GameObject Settings;
    PlayerList playerList;

    // Start is called before the first frame update

    private void Awake()
    {
        gameManager = GameManager.FindInstance();
    }
    void Start()
    {

    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (player == runner.LocalPlayer)
        {

            Debug.Log("Spawning Player");
            runner.Spawn(playerPrefab, Utils.GetRandomSpawnPoint(), Quaternion.identity, player,OnBeforeSpawn);
            Debug.Log(gameManager.GetRunner().SessionInfo.ToString());
        }
        Debug.Log(gameManager.GetUserStatus());

        StartCoroutine(WaitPlayer());
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        StartCoroutine(WaitPlayer());
    }

    IEnumerator WaitPlayer()
    {
        yield return new WaitForSeconds(2);
        if (gameManager.GetUserStatus() == UserStatus.InPause)
        {

            if (gameManager.GetUserRole() == UserRole.Admin)
            {
                Debug.Log("llegooooooooooooooooooooooooooooooooooooo");
                playerList = GameObject.Find("Menus").transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<PlayerList>();
                Debug.Log(GameObject.Find("Menus").transform.GetChild(0).GetChild(0).GetChild(3));
                playerList.ListPlayers();
            }

        }
    }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        Debug.Log("Llega input" +  input.ToString());
        if (characterInputHandler == null && NetworkPlayer.Local != null) 
            characterInputHandler = NetworkPlayer.Local.GetComponent<CharacterInputHandler>();

        if(characterInputHandler != null)
        {
            input.Set(characterInputHandler.GetNetworkInput());
            //Debug.Log("seteado");
        }
    }


    public void OnBeforeSpawn(NetworkRunner runner, NetworkObject obj)
    {
        obj.GetComponent<NetworkPlayer>().avatar = gameManager.GetAvatarNumber();
        obj.GetComponent<NetworkPlayer>().ActorID = gameManager.GetRunner().LocalPlayer;
        obj.GetComponent<NetworkPlayer>().nickname = gameManager.GetUsername();
        gameManager.SetCurrentPlayer(obj.gameObject);
        obj.transform.GetChild(2).gameObject.SetActive(true);
    }







    public void OnConnectedToServer(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        throw new NotImplementedException();
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        throw new NotImplementedException();
    }

    

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    

   

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        throw new NotImplementedException();
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        throw new NotImplementedException();
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        throw new NotImplementedException();
    }

    
}
