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
    private UserManager userManager;
    public GameObject Settings;
    PlayerList playerList;

    // Start is called before the first frame update

    private void Awake()
    {
        gameManager = GameManager.FindInstance();
        userManager = UserManager.FindInstance();
    }

    /// <summary>
    /// When a player enters, spawns with the selected prefab and gives it the network properties.
    /// Initialice WaitPlayer
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="player"></param>
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (player == runner.LocalPlayer)
        {
            GameObject handler = GameObject.Find("NetworkRunnerHandler");
            Debug.Log("Spawning Player");
            runner.Spawn(playerPrefab, handler.transform.position, Quaternion.identity, player,OnBeforeSpawn);
            Debug.Log(gameManager.GetRunner().SessionInfo.ToString());
        }
        Debug.Log(gameManager.GetUserStatus());

        StartCoroutine(WaitPlayer());
    }
    /// <summary>
    /// Detects that the user has left, to remove them from the list of players.
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="player"></param>
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        StartCoroutine(WaitPlayer());
    }

    IEnumerator WaitPlayer()
    {
        yield return new WaitForSeconds(2);
        playerList = GameObject.Find("Menus").transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<PlayerList>();
        Debug.Log(GameObject.Find("Menus").transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2));
        playerList.ListPlayers();
    }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
       
        if (characterInputHandler == null && NetworkPlayer.Local != null) 
            characterInputHandler = NetworkPlayer.Local.GetComponent<CharacterInputHandler>();

        if(characterInputHandler != null)
        {
            input.Set(characterInputHandler.GetNetworkInput());
         
        }
    }

    /// <summary>
    /// Initialises the individual properties of each user
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="obj"></param>
    public void OnBeforeSpawn(NetworkRunner runner, NetworkObject obj)
    {
        obj.GetComponent<NetworkPlayer>().avatar = gameManager.GetAvatarNumber();
        obj.GetComponent<NetworkPlayer>().ActorID = gameManager.GetRunner().LocalPlayer;
        obj.GetComponent<NetworkPlayer>().nickname = userManager.Username;
        gameManager.SetCurrentPlayer(obj.gameObject);
        obj.transform.GetChild(2).gameObject.SetActive(true);
    }







    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("Connected to a room");
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
        Debug.Log("Scene load");
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        Debug.Log("Loading scene");
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        throw new NotImplementedException();
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log("Leaving Room");
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        throw new NotImplementedException();
    }

    
}
