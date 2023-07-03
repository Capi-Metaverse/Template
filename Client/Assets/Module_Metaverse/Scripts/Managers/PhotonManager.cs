using Fusion;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using Fusion.Sockets;
using System;


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
public class PhotonManager : MonoBehaviour, INetworkRunnerCallbacks
{

    //Managers

    private MSceneManager mSceneManager;

    //Runner, JUST ONE PER USER/ROOM
    [SerializeField] private NetworkRunner _runner;

    public NetworkRunner Runner { get => _runner; set => _runner = value; }

    //PhotonManager

    [SerializeField] private GameObject _currentPlayer;
    public GameObject CurrentPlayer { get => _currentPlayer; set => _currentPlayer = value; }

    //Name of the PhotonRoom
    private string _roomName;
    public string RoomName { get => _roomName; set => _roomName = value; }

    //Number of players
    private int _playerCount;

    public int PlayerCount { get => _playerCount; set => _playerCount = value; }

    //Multiplayer server Lobby
    public const string LOBBY_NAME = "Main";

  

    //Connection Status
    [SerializeField] private ConnectionStatus _connectionStatus;
    public ConnectionStatus ConnectionStatus { get => _connectionStatus; set => _connectionStatus = value; }

    //User Status
    [SerializeField] private UserStatus _userStatus;
    public UserStatus UserStatus { get => _userStatus; set => _userStatus = value; }

    //Static function to get the singleton
    public static PhotonManager FindInstance()
    {
        return FindObjectOfType<PhotonManager>();
    }

    //The Lobby Manager from Lobby Scene
    private LobbyManager _lobbyManager;


    //GameManager
    public int avatarNumber = 0;
    public bool CameraBool = false;

    //GameManager
    [SerializeField] private InputManager inputManager;
    //Initialization
    private void Awake()
    {
        //When this component awake, it get the others game managers
        PhotonManager[] managers = FindObjectsOfType<PhotonManager>();

        //Check if there is more managers
        if (managers != null && managers.Length > 1)
        {
            // There should never be more than a single App container in the context of this sample.
            Destroy(gameObject);
            return;

        }
    }

    private void Start()
    {
        mSceneManager = MSceneManager.FindInstance();
    }

    //List of rooms
    private List<SessionInfo> sessionList;

    /// <summary>
    /// Add the NetworkRunner to this object 
    /// </summary>
    private void Connect()
    {

        if (Runner == null)
        {

            //Initializes the runner
            ConnectionStatus = ConnectionStatus.Connecting;
            GameObject go = new GameObject("Session");


            Runner = go.AddComponent<NetworkRunner>();
            Runner.AddCallbacks(this);

        }
    }

    /// <summary>
    /// Disconnects the runner 
    /// </summary>
    /// <returns></returns>
    public async Task Disconnect()
    {
        if (Runner != null)
        {
            //Disconnects the runner
            ConnectionStatus = ConnectionStatus.Disconnected;
            await _runner.Shutdown();
            Runner = null;

        }
    }

    //SceneManager
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


    /// <summary>
    /// Function to enter the Lobby, load Lobby if fail, load 1.Start
    /// </summary>
    /// <returns></returns>
    public async Task EnterLobby()
    {
        //We connect to photonFusion
        Connect();

        //We connect to the Lobby
        ConnectionStatus = ConnectionStatus.EnteringLobby;
        var result = await Runner.JoinSessionLobby(SessionLobby.Custom, LOBBY_NAME);

        //If the connection fails...
        if (!result.Ok)
        {
            ConnectionStatus = ConnectionStatus.Failed;

            await Disconnect();
            //Mostrar error screen
            //If it's not Playfab, change to another scene

            mSceneManager.LoadLogin();

            return;
        }
        //Change this line maybe
        _lobbyManager.setLobbyButtons(true);
        UserStatus = UserStatus.PreLobby;
    }


    /// <summary>
    /// Create a session/room Correct
    /// </summary>
    /// <param name="props"></param>
    public async void CreateSession(SessionProps props)
    {
        await StartSession(GameMode.Shared, props);
    }


    //PhotonManager
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


    //PhotonManager
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

        ConnectionStatus = ConnectionStatus.Starting;

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

        PlayerCount = 10;
        RoomName = props.RoomName.ToUpper();

        //Maybe refactor this part Add Player in setPlayerPanel?


        if (!result.Ok)
        {
            ConnectionStatus = ConnectionStatus.Failed;

            await Disconnect();

            mSceneManager.LoadLogin();

            return;
        }

        if (UserStatus == UserStatus.PreLobby)
        {
            Debug.Log("[Photon-GameManager] Entering Lobby");
            //Indicate LobbyManager to change the panel
            _lobbyManager.SetPlayerPanel(props.RoomName, _runner);
            UserStatus = UserStatus.InLobby;
        }

    }

    //PhotonManager
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

    //PhotonManager
    /// <summary>
    /// Function to Leave Room
    /// </summary>
    public async void LeaveSession()
    {
        await Disconnect();
        await EnterLobby();
    }
    //PhotonManager
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

        RoomName = new string(sessionName.Where(c => char.IsLetter(c) || char.IsDigit(c)).ToArray());
        RoomName = RoomName.ToUpper();
        //We change to the new map
        //THIS WILL BE THE LOBBY WHEN IT'S ENDED
        mSceneManager.LoadMain();
        Debug.Log("Creating session");
    }

    //PhotonManager
    /// <summary>
    /// Set up the custom room within the scenes, so you can join the scenes.
    /// </summary>
    /// <param name="sessionName"></param>
    /// <param name="playerNumber"></param>
    /// <param name="map"></param>
    public async void StartCustomGame(string sessionName, int playerNumber, string scene)
    {
        await Disconnect();
        RoomName = new string(sessionName.Where(c => char.IsLetter(c) || char.IsDigit(c)).ToArray());
        RoomName = RoomName.ToUpper();
        PlayerCount = playerNumber;
        //We change to the respective map
        mSceneManager.LoadScene(scene);
    }

    //PhotonManager
    /// <summary>
    /// Joins the custom game, previously created
    /// </summary>
    /// <param name="sessionName"></param>
    public async void JoinCustomGame(string sessionName)
    {
        await Disconnect();
        RoomName = new string(sessionName.Where(c => char.IsLetter(c) || char.IsDigit(c)).ToArray());
        PlayerCount = 4;
        Connect();

        //Nos unimos al lobby de custom
        var result = await _runner.JoinSessionLobby(SessionLobby.Custom, "Lobby_Play");

        foreach (SessionInfo item in sessionList)
        {

            Debug.Log(item.Properties.ToString());

            if (item.Properties["RoomName"].Equals(sessionName))
            {
                //currentMap = item.Properties["StartMap"];
                PlayerCount = item.Properties["PlayerLimit"];
                break;
            }

        }





        //We change to the respective map
        //SceneManager.LoadSceneAsync(currentMap);
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

    //PhotonManager
    public async void ChangeScene(string scene)
    {
        await Disconnect();
        mSceneManager.LoadScene(scene);

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

            //Any time a player enters the room we activate the FriendMark on minimap if it´s a friend
            //List<Friend> listFriend = GameObject.Find("Menus").transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).gameObject.GetComponent<FriendManager>().GetFriendsConfirmedListAsync();
            //GameManager.FindInstance().GetComponent<CharacterInputHandler>().ActivateFriendsMarker(listFriend);

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
