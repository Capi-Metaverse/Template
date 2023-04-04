using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
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
    InPause
}

public enum UserRole
{
    Admin,
    Moderator,
    Client,
    Employee
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

    public GameObject currentPlayer;

    //The Lobby Manager from Lobby Scene
    private LobbyManager _lobbyManager;

    //User username
    public string username = "Pepito";
    //User ID
    public string userID;
    //User Role
    public string UserRole;


    public string mapName;

    //Static function to get the singleton
    public static GameManager FindInstance()
    {
        return FindObjectOfType<GameManager>();
    }

    //Name of the Lobby of the game
    private const string LOBBY_NAME = "Main";


    //Connection Status
    public ConnectionStatus ConnectionStatus { get; private set; }

    public UserStatus UserStatus { get; private set; }

    public UserRole UserRole { get; private set; }

    public int avatarNumber = 0;
    public GameObject[] playerPrefabs;



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
    
    //This function sets the LobbyManager and enter the Lobby
    public async void SetLobbyManager(LobbyManager lobbyManager)
    {
        this._lobbyManager = lobbyManager;
        await EnterLobby();
        
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
    public async void Disconnect()
    {
        if (_runner != null)
        {
            //Disconnects the runner
            SetConnectionStatus(ConnectionStatus.Disconnected);
            await _runner.Shutdown();
            _runner = null;

        }
    }

    //Function to enter the Lobby
    public async Task EnterLobby()
    {
       
        //We connect to photonFusion
        Connect();
        
        //We connect to the Lobby
        SetConnectionStatus(ConnectionStatus.EnteringLobby);
        var result = await _runner.JoinSessionLobby(SessionLobby.Custom, LOBBY_NAME);
        
        //If the connection fails...
        if (!result.Ok)
        {
            SetConnectionStatus(ConnectionStatus.Failed);
        }

        _lobbyManager.setLobbyButtons(true);
        SetUserStatus(UserStatus.PreLobby);
    }
  

    //Create a session/room
    public async void CreateSession(SessionProps props)
    {
        await StartSession( GameMode.Shared, props);
    }

    //Join a session/room
    public async void JoinSession(SessionInfo info)
    {
        SessionProps props = new SessionProps(info.Properties);
        props.PlayerLimit = info.MaxPlayers;
        props.RoomName = info.Name;
       await StartSession( GameMode.Shared, props);
    }

    //Function to create the session / room
    public async Task StartSession(GameMode mode, SessionProps props, bool disableClientSessionCreation = true)
    {
       Connect();

        SetConnectionStatus(ConnectionStatus.Starting);

        Debug.Log($"Starting game with session {props.RoomName}, player limit {props.PlayerLimit}");
        _runner.ProvideInput = mode != GameMode.Server;
        StartGameResult result = await _runner.StartGame(new StartGameArgs
        {
            GameMode = mode,
            CustomLobbyName = LOBBY_NAME,
            SceneManager = _loader,
            SessionName = props.RoomName,
            PlayerCount = 10,
            SessionProperties = props.Properties,
           
        });

        if (UserStatus == UserStatus.PreLobby)
        {
            Debug.Log("Entering Lobby");
            //Indicate LobbyManager to change the panel
            _lobbyManager.SetPlayerPanel(props.RoomName);
            _lobbyManager.setLobbyButtons(false);
            _lobbyManager.AddPlayer();

            SetUserStatus(UserStatus.InLobby);
        }

        else if (UserStatus == UserStatus.InLobby)
        {
            //Lógica para entrar a SalaLobby
        }



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
    public void SetUserStatus(UserStatus status, string reason = "")
    {
        if (UserStatus == status)
            return;
       UserStatus = status;
    }

    public void SetUserRole(UserRole role, string reason = "")
    {
        if (UserRole == role)
            return;
        UserRole = role;
    }

    //Function that updates the list of sessions
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        //Because it seems this doesn't work like PUN2, i think is better update the whole panel

        _lobbyManager.SetSessionList(sessionList);
    }


    //Function that spawns the Player Item
    public void SpawnPlayerItem(PlayerItem player)
    {
     PlayerItem.Spawn(_runner, player);
    }

    //Function to Leave Room
    public async void LeaveSession()
    {
       Disconnect();
      

        
        await EnterLobby();

       
    }
    public void DisconnectSession()
    {
        Disconnect();
      
    }
    //Function that executes when a user clicks on Join in the Lobby
    public void StartGame(string sessionName, int avatarNumber)
    {
        //We disconnect the actual runner
         Disconnect();
        this.avatarNumber = avatarNumber;
        mapName = sessionName;
        //We change to the new map
        SceneManager.LoadSceneAsync("Mapa1");
        Debug.Log("Creating session");


    }

    /* Function that changes the map to another
     * For example Map1 --> Map2 // Lobby --> HUBValencia
     * 
     * It disconnects the current Network Runner and change the scene to the new map.
     * It's not needed to start the new Runner because the Runner Handler of the scene is responsible for initialising it.
     * 
     */
    public void ChangeMap(string map)
    {
        Disconnect();
        SceneManager.LoadSceneAsync(map);

    }



    //Runner Get Set

    public NetworkRunner GetRunner() { return _runner; }

    public void SetRunner(NetworkRunner runner)
    {
        _runner = runner;
    }




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
            Debug.Log("A user has joined to the room");
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

    public GameObject GetCurrentPlayer()
    {
        return currentPlayer;
    }
}
