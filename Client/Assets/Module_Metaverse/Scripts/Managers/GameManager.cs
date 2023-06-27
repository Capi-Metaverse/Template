using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;

using Newtonsoft.Json.Linq;

using Unity.VisualScripting;

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

public class GameManager : SimulationBehaviour, INetworkRunnerCallbacks
{

    /*
     * The game manager is the main of the App, it controls all the user connections.
     * 
     * 
     * 
     */

    //Managers

    //This SceneManager is going to change between scenes and is going to put a loading screen between them.
    [SerializeField] private NetworkSceneManagerBase _loader;

    //Runner, JUST ONE PER USER/ROOM
    //It's like PhotonNetwork.somefunction() in PUN2
    [SerializeField] private NetworkRunner _runner;

    [SerializeField] private InputManager inputManager;

    private GameObject currentPlayer;

    //The Lobby Manager from Lobby Scene
    private LobbyManager _lobbyManager;

    private List<SessionInfo> sessionList;

    private string roomName; //This is the RoomName
    public int playerCount;
    public string currentMap;

    public GameObject Settings;



    //Static function to get the singleton
    public static GameManager FindInstance()
    {
        return FindObjectOfType<GameManager>();
    }

    //Name of the Lobby of the game
    private const string LOBBY_NAME = "Main";


    //Connection Status
    [SerializeField] private ConnectionStatus ConnectionStatus { get; set; }

    [SerializeField] public UserStatus UserStatus { get; set; }

    private int avatarNumber = 0;

    public bool CameraBool = false;

    //Initialization Correct
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


            //Don't destroy the Game Manager
            DontDestroyOnLoad(gameObject);

            //To indicate we are not in the Lobby
            SetUserStatus(UserStatus.PreLobby);

            //Change to the login scene ONLY IF THE LOGIN IS NOT THE FIRST SCENE
            //SceneManager.LoadSceneAsync( _startScene);

        }
    }

    /// <summary>
    /// This function sets the LobbyManager and enter the Lobby
    /// </summary>
    /// <param name="lobbyManager"></param>
    public async void SetLobbyManager(LobbyManager lobbyManager)
    {
        if (inputManager == null) inputManager = this.gameObject.AddComponent<InputManager>();


        this._lobbyManager = lobbyManager;
        await EnterLobby();

    }
    //AWAKE -> Intro -> Lobby -> Session

    /// <summary>
    /// Add the NetworkRunner to this object 
    /// </summary>
    private void Connect()
    {

        if (_runner == null)
        {

            //Initializes the runner
            SetConnectionStatus(ConnectionStatus.Connecting);
            GameObject go = new GameObject("Session");


            _runner = go.AddComponent<NetworkRunner>();
            _runner.AddCallbacks(this);

        }
    }

    /// <summary>
    /// Disconnects the runner 
    /// </summary>
    /// <returns></returns>
    public async Task Disconnect()
    {
        if (_runner != null)
        {
            //Disconnects the runner
            SetConnectionStatus(ConnectionStatus.Disconnected);
            await _runner.Shutdown();
            _runner = null;

        }
    }

    /// <summary>
    /// Function to enter the Lobby, load Lobby if fail, load 1.Start
    /// </summary>
    /// <returns></returns>
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

            await Disconnect();
            //Mostrar error screen
            SceneManager.LoadScene("1.Start");

            return;
        }
        _lobbyManager.setLobbyButtons(true);
        SetUserStatus(UserStatus.PreLobby);
    }


    /// <summary>
    /// Create a session/room Correct
    /// </summary>
    /// <param name="props"></param>
    public async void CreateSession(SessionProps props)
    {
        await StartSession(GameMode.Shared, props);
    }

    /// <summary>
    /// Join a session/room Correct, Maybe improve props in the future
    /// </summary>
    /// <param name="info"></param>
    public async void JoinSession(SessionInfo info)
    {
        SessionProps props = new SessionProps(info.Properties);
        props.PlayerLimit = info.MaxPlayers;
        props.RoomName = info.Name;
        await StartSession(GameMode.Shared, props);
    }

    /// <summary>
    /// Function to create the session / room , Set properties to Room, if fail load 1.Start
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="props"></param>
    /// <param name="disableClientSessionCreation"></param>
    /// <returns></returns>
    public async Task StartSession(GameMode mode, SessionProps props, bool disableClientSessionCreation = true)
    {
        Connect();

        SetConnectionStatus(ConnectionStatus.Starting);

        Debug.Log($"[Photon-GameManager] Starting game with session {props.RoomName}, player limit {props.PlayerLimit}");
        _runner.ProvideInput = mode != GameMode.Server;
        StartGameResult result = await _runner.StartGame(new StartGameArgs
        {
            GameMode = mode,
            CustomLobbyName = LOBBY_NAME,
            SceneManager = _runner.AddComponent<NetworkSceneManagerDefault>(),
            SessionName = props.RoomName.ToUpper(),
            PlayerCount = 10,
            SessionProperties = props.Properties,

        });

        playerCount = 10;
        currentMap = "LobbyOficial";
        roomName = props.RoomName.ToUpper();

        //Maybe refactor this part Add Player in setPlayerPanel?


        if (!result.Ok)
        {
            SetConnectionStatus(ConnectionStatus.Failed, result.ShutdownReason.ToString());

            await Disconnect();

            SceneManager.LoadScene("1.Start");

            return;
        }

        if (UserStatus == UserStatus.PreLobby)
        {
            Debug.Log("[Photon-GameManager] Entering Lobby");
            //Indicate LobbyManager to change the panel
            _lobbyManager.SetPlayerPanel(props.RoomName, _runner);
            SetUserStatus(UserStatus.InLobby);
        }

    }

    /// <summary>
    /// Set the user connection
    /// </summary>
    /// <param name="status"></param>
    /// <param name="reason"></param>
    private void SetConnectionStatus(ConnectionStatus status, string reason = "")
    {
        if (ConnectionStatus == status)
            return;
        ConnectionStatus = status;

        Debug.Log($"[Photon-GameManager] ConnectionStatus={status} {reason}");
    }

    public ConnectionStatus GetConnectionStatus()
    {
        return this.ConnectionStatus;

    }

    /// <summary>
    /// Set the user status
    /// </summary>
    /// <param name="status"></param>
    /// <param name="reason"></param>
    public void SetUserStatus(UserStatus status, string reason = "")
    {
        if (UserStatus == status)
            return;
        UserStatus = status;
    }

    public UserStatus GetUserStatus()
    {
        return this.UserStatus;

    }


    public GameManager GetGameManager()
    {
        return this;
    }

    /// <summary>
    /// Function that updates the list of sessions
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="sessionList"></param>
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        //Because it seems this doesn't work like PUN2, i think is better update the whole panel

        _lobbyManager.SetSessionList(sessionList);
        this.sessionList = sessionList;
    }


    /// <summary>
    /// Function to Leave Room
    /// </summary>
    public async void LeaveSession()
    {
        await Disconnect();
        await EnterLobby();
    }
    /// <summary>
    /// Function that executes when a user clicks on Join in the Lobby
    /// </summary>
    /// <param name="sessionName"></param>
    /// <param name="avatarNumber"></param>
    public async void StartGame(string sessionName, int avatarNumber)
    {
        //We disconnect the actual runner
        await Disconnect();
        this.avatarNumber = avatarNumber;

        roomName = new string(sessionName.Where(c => char.IsLetter(c) || char.IsDigit(c)).ToArray());
        roomName = roomName.ToUpper();
        //We change to the new map
        //THIS WILL BE THE LOBBY WHEN IT'S ENDED
        SceneManager.LoadSceneAsync("LobbyOficial");
        Debug.Log("Creating session");
    }
    /// <summary>
    /// Set up the custom room within the scenes, so you can join the scenes.
    /// </summary>
    /// <param name="sessionName"></param>
    /// <param name="playerNumber"></param>
    /// <param name="map"></param>
    public async void StartCustomGame(string sessionName, int playerNumber, string map)
    {
        await Disconnect();
        roomName = new string(sessionName.Where(c => char.IsLetter(c) || char.IsDigit(c)).ToArray());
        roomName = roomName.ToUpper();
        currentMap = map;
        playerCount = playerNumber;
        //We change to the respective map
        SceneManager.LoadSceneAsync(map);
    }
    /// <summary>
    /// Joins the custom game, previously created
    /// </summary>
    /// <param name="sessionName"></param>
    public async void JoinCustomGame(string sessionName)
    {
        await Disconnect();
        roomName = new string(sessionName.Where(c => char.IsLetter(c) || char.IsDigit(c)).ToArray());
        currentMap = "LobbyOficial";
        playerCount = 4;
        Connect();

        //Nos unimos al lobby de custom
        var result = await _runner.JoinSessionLobby(SessionLobby.Custom, "Lobby_Play");

        foreach (SessionInfo item in sessionList) {

            Debug.Log(item.Properties.ToString());

            if (item.Properties["RoomName"].Equals(sessionName))
            {
                currentMap = item.Properties["StartMap"];
                playerCount = item.Properties["PlayerLimit"];
                break;
            }

        }





        //We change to the respective map
        SceneManager.LoadSceneAsync(currentMap);
    }

    /* Function that changes the map to another
     * For example Map1 --> Map2 // Lobby --> HUBValencia
     * 
     * It disconnects the current Network Runner and change the scene to the new map.
     * It's not needed to start the new Runner because the Runner Handler of the scene is responsible for initialising it.
     * 
     */
    /// <summary>
    ///  Function that changes the map to another
    /// For example Map1 --> Map2 // Lobby --> HUBValencia
    /// It disconnects the current Network Runner and change the scene to the new map.
    /// It's not needed to start the new Runner because the Runner Handler of the scene is responsible for initialising it.
    /// </summary>
    /// <param name="map"></param>


    public async void ChangeMap(string map)
    {
        await Disconnect();
        SceneManager.LoadSceneAsync(map);

    }



    /// <summary>
    /// Runner Get Set
    /// </summary>
    /// <returns></returns>
   
    public NetworkRunner GetRunner() { return _runner; }

    public void SetRunner(NetworkRunner runner)
    {
        _runner = runner;
    }

    //CurrentPlayer Get Set 
    //This is the GameObject IN-GAME
    public GameObject GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public void SetCurrentPlayer(GameObject currentPlayer)
    {
        this.currentPlayer = currentPlayer;
    }

    //AvatarNumber Get Set

    public int GetAvatarNumber()
    {
        return this.avatarNumber;
    }

    public void SetAvatarNumber(int avatarNumber)
    {
        this.avatarNumber = avatarNumber;
    }


    public string GetRoomName()
    {
        return this.roomName;
    }

    public void SetRoomName(string roomName)
    {
        this.roomName = roomName;
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
        //Do nothing
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {

        if (UserStatus == UserStatus.InLobby)
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
        Debug.Log("Closing runner");
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        throw new NotImplementedException();
    }

}


