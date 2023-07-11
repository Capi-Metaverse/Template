using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;
using System.Linq;
using Manager;

namespace Player
{
    public class NetworkRunnerHandler : MonoBehaviour
    {

        //Network Runner Prefab
        public NetworkRunner networkRunnerPrefab;

        //Network Runner Script
        NetworkRunner NetworkRunner;
        //Photon Manager
        PhotonManager PhotonManager;

        //Map
        public string map;

        private void Awake()
        {
            PhotonManager = PhotonManager.FindInstance();
        }

        private void Start()
        {
            NetworkRunner = Instantiate(networkRunnerPrefab);
            NetworkRunner.name = "Network runner";

            var clientTask = InitializeNetworkRunner(NetworkRunner, GameMode.Shared, NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);


            Debug.Log("Server NetworkRunner Started");
        }

        /// <summary>
        /// Function that initializes the network runner
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="gameMode"></param>
        /// <param name="address"></param>
        /// <param name="scene"></param>
        /// <param name="initialized"></param>
        /// <returns></returns>

        protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, NetAddress address, SceneRef scene, Action<NetworkRunner> initialized)
        {
            var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

            if (sceneManager == null)
            {

                sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
            }

            runner.ProvideInput = true;
            PhotonManager.FindInstance().Runner = runner;

            SessionProps props = new SessionProps();
            props.StartMap = MSceneManager.FindInstance().CurrentScene;
            props.RoomName = PhotonManager.RoomName;
            props.AllowLateJoin = true;
            props.PlayerLimit = PhotonManager.PlayerCount;

            return runner.StartGame(new StartGameArgs
            {
                GameMode = gameMode,
                Address = address,
                Scene = scene,
                CustomLobbyName = "Lobby_Play",
                SessionName = PhotonManager.RoomName + "-" + map,
                PlayerCount = PhotonManager.PlayerCount,
                Initialized = initialized,
                SceneManager = sceneManager,
                SessionProperties = props.Properties
            }); ;

        }
    }
}
