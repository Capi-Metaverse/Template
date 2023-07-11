using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using Fusion.Sockets;
using System.Linq;
using Unity.Entities.UniversalDelegates;
using Manager;
using Player;
using NetworkPlayer = Player.NetworkPlayer;

namespace Settings
{
    public class PlayerList : MonoBehaviour
    {
        public GameObject PlayerItemPrefabSettings;
        public GameObject PlayerListSettings;
        private GameObject gameObjectPlayer;
        private GameManager gameManager;
        [SerializeField] private Toggle muteButton;
        private bool mute = false;

        public GameObject GameObjectPlayer { get => gameObjectPlayer; set => gameObjectPlayer = value; }

        public void Start()
        {
            gameManager = GameManager.FindInstance();
            UserManager userManager = UserManager.FindInstance();
            if (userManager.UserRole == UserRole.Admin)
            {
                muteButton.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// List Player with his names in PlayFab and Get his ID
        /// </summary>
        public void ListPlayers()
        {
            //Destroys the former list
            foreach (Transform child in PlayerListSettings.transform)
            {
                Destroy(child.gameObject);
            }
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            //We get the player list
            foreach (GameObject player in players)
            {
                //We get the network player

                NetworkPlayer networkPlayer = player.GetComponent<NetworkPlayer>();


                //We create the userItem object
                GameObject userItem = (GameObject)Instantiate(PlayerItemPrefabSettings);

                userItem.transform.SetParent(PlayerListSettings.transform);
                userItem.transform.localScale = Vector3.one;

                //We configure the Nickname
                TMP_Text PlayerNameText = userItem.transform.GetChild(0).GetComponent<TMP_Text>();
                PlayerNameText.text = networkPlayer.nickname.ToString();

                //We get the userItem component

                userItem.GetComponent<UserListItem>().NumActor = networkPlayer.ActorID;
                userItem.GetComponent<UserListItem>().GameObjectPlayer = player;
            }

        }

        public void MuteAll()
        {
            int PlayerID = PhotonManager.FindInstance().CurrentPlayer.GetComponent<NetworkPlayer>().ActorID;
            mute = !mute;
            RPCManager.RPC_MuteAllPlayers(PhotonManager.FindInstance().Runner, mute, PlayerID);
        }
    }
}