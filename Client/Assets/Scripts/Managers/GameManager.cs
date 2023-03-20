using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Collections.Unicode;


//Status of the connection WIP (Some status are not necessary)
public enum ConnectionStatus
{
    Disconnected,
    Connecting,
    Connected,
    EnteringLobby,
    InLobby,
    Starting,
    Started,
    Failed,
}

//Status of the location of the user WIP (It can increase)
public enum UserStatus
{
    PreLobby,
    InLobby,
    InGame,

}

//Require adds the component as a dependency
[RequireComponent(typeof(NetworkSceneManagerBase))]
public class GameManager : MonoBehaviour, INetworkRunnerCallbacks
{

    /*
     * The game manager is the main of the App, it controls all the user connections.
     * 
     * 
     * 
     */

    //Managers

    //Init scene of the game (Not need it right now)
    [SerializeField] private SceneReference _startScene;

    //This SceneManager is going to change between scenes and is going to put a loading screen between them.
    [SerializeField] private NetworkSceneManagerBase _loader;

    //Runner, JUST ONE PER USER/ROOM
    //It's like PhotonNetwork.somefunction() in PUN2
    [SerializeField] private NetworkRunner _runner;

    //The Lobby Manager from Lobby Scene
    public LobbyManager _lobbyManager;

    //User username
    public string username = "Pepito";
    //User ID
    public string userID;

    //Current Game Object

    public GameObject current;

    //Static function to get the singleton
    public static GameManager FindInstance()
    {
        return FindObjectOfType<GameManager>();
    }

    //Name of the Lobby of the game
    private string _lobbyId = "Lobbyprueba";


    //Connection Status
    public ConnectionStatus ConnectionStatus { get; private set; }

    public UserStatus UserStatus { get; private set; }



    //Initialization
    private void Awake()
    {
        //When this component awake, it get the others game managers
        GameManager[] managers = FindObjectsOfType<GameManager>();
      
        //Check if there is more managers
        if (managers != null && managers.Length > 1)
        {
            // There should never be more than a single App container in the context of this sample.
            Destroy(gameObject);
            return;


        }

        //If the scene loader is null, initializates it and change to the start scene.
        if (_loader == null)
        {
            _loader = GetComponent<NetworkSceneManagerBase>();

            //Don't destroy the Game Manager
            DontDestroyOnLoad(gameObject);

            //To indicate we are not in the Lobby
            SetUserStatus(UserStatus.PreLobby);

            //Change to the login scene ONLY IF THE LOGIN IS NOT THE FIRST SCENE
            //SceneManager.LoadSceneAsync( _startScene);
       
        }
    }
    
    //This function sets the LobbyManager
    public async void setLobbyManager(LobbyManager lobbyManager)
    {
        this._lobbyManager = lobbyManager;
        await EnterLobby("Main");
    }
    //AWAKE -> Intro -> Lobby -> Session

    //Add the NetworkRunner to this object
    private void Connect()
    {
        
        if (_runner == null)
        {
            //Initializes the runner
            SetConnectionStatus(ConnectionStatus.Connecting);
            GameObject go = new GameObject("Session");
            go.transform.SetParent(transform);

            _runner = go.AddComponent<NetworkRunner>();
            _runner.AddCallbacks(this);
        }
    }

    //Disconnects the runner
    public void Disconnect()
    {
        if (_runner != null)
        {
            //Disconnects the runner
            SetConnectionStatus(ConnectionStatus.Disconnected);
            _runner.Shutdown();
            _runner = null;
        }
    }

    //Function to enter the Lobby
    public async Task EnterLobby(string lobbyId)
    {
       
        //We connect to photonFusion
        Connect();

        //We get the LobbyID to connect
        _lobbyId = lobbyId;
        

        //We connect to the Lobby
        SetConnectionStatus(ConnectionStatus.EnteringLobby);
        var result = await _runner.JoinSessionLobby(SessionLobby.Custom, lobbyId);
        
        //If the connection fails...
        if (!result.Ok)
        {
         
            SetConnectionStatus(ConnectionStatus.Failed);
           
        }
    }

    //Create a session/room
    public void CreateSession(SessionProps props)
    {
        StartSession( GameMode.Shared, props);
    }

    //Join a session/room
    public void JoinSession(SessionInfo info)
    {
        SessionProps props = new SessionProps(info.Properties);
        props.PlayerLimit = info.MaxPlayers;
        props.RoomName = info.Name;
        StartSession( GameMode.Shared, props);
    }

    //Function to create the session / room
    public async void StartSession(GameMode mode, SessionProps props, bool disableClientSessionCreation = true)
    {
        Connect();

        SetConnectionStatus(ConnectionStatus.Starting);

        if (UserStatus == UserStatus.PreLobby)
        {

            SetUserStatus(UserStatus.InLobby);
           
        }
        

        Debug.Log($"Starting game with session {props.RoomName}, player limit {props.PlayerLimit}");
        _runner.ProvideInput = mode != GameMode.Server;
        StartGameResult result = await _runner.StartGame(new StartGameArgs
        {
            GameMode = mode,
            CustomLobbyName = _lobbyId,
            SceneManager = _loader,
            SessionName = props.RoomName,
            PlayerCount = 4,
            SessionProperties = props.Properties,
           
        });

        //If the user is not in the Lobby Room, it change to Lobby Status

        Debug.Log(_runner.LocalPlayer);

        //Indicate LobbyManager to change the panel
        _lobbyManager.setPlayerPanel(props.RoomName);
        _lobbyManager.addPlayer();
        



        if (!result.Ok)
            SetConnectionStatus(ConnectionStatus.Failed, result.ShutdownReason.ToString());
    }



    //Debug function to know the user connection
    private void SetConnectionStatus(ConnectionStatus status, string reason = "")
    {
        if (ConnectionStatus == status)
            return;
        ConnectionStatus = status;

        /*
        if (!string.IsNullOrWhiteSpace(reason) && reason != "Ok")
        {
            _errorBox.Show(status, reason);
        }
        */

        Debug.Log($"ConnectionStatus={status} {reason}");
    }

    //Debug function to know the user status
    private void SetUserStatus(UserStatus status, string reason = "")
    {
        if (UserStatus == status)
            return;
       UserStatus = status;
    }

    //Function that updates the list of sessions
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        //Because it seems this doesn't work like PUN2, i think is better update the whole panel

        _lobbyManager.setSessionList(sessionList);
    }

    public void spawnPlayerItem(PlayerItem player)
    {
     player.Spawn(_runner, player);

    }

    public void despawnPlayerItem(PlayerItem player)
    {
        _runner.Despawn(player.networkObject);
    }

    //Function to Leave Room

    public async void LeaveSession()
    {
        Disconnect();
        _lobbyManager.cleanSessions();
        await EnterLobby(_lobbyId);
    }
    


    public NetworkRunner GetRunner() { return _runner; }




    //Callback when the user joins to a Server/Host
    public void OnConnectedToServer(NetworkRunner runner)
    {
        
    }



    //Callback when the user fails to join to a Server/Host
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        request.Accept();
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

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if(UserStatus == UserStatus.InLobby)
        {
            //Do nothing
        }
        else
        {
            //In game input
        }
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
       
        if(UserStatus == UserStatus.InLobby)
        {
            Debug.Log("HOLA");
            //We indicate to the LobbyManager that he has a new user
            

        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {

    
            Debug.Log("A user has disconnected from the room");

       
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

    

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        throw new NotImplementedException();
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        throw new NotImplementedException();
    }


}
