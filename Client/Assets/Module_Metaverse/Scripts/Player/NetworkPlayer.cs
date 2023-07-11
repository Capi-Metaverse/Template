using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using Manager;

namespace Player
{
    public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
    {
        //Managers
        private UserManager userManager;
        private PhotonManager photonManager;
        public CharacterInputHandler inputHandler;

        //Player Info 
        public static NetworkPlayer Local { get; set; }
        [Networked]
        public int avatar { get; set; }

        public TextMeshProUGUI playerNicknameTM;
        [Networked]
        public string playfabIdentity { get; set; }
        [Networked(OnChanged = nameof(OnNickNameChanged))]
        public NetworkString<_16> nickname { get; set; }
        public NetworkString<_16> playfabId { get; set; }
        [Networked] public int ActorID { get; set; }


        //Avatar Prefabs
        public GameObject[] playerPrefabs;
        //Animator
        public Animator animator;

        private void Awake()
        {
            userManager = UserManager.FindInstance();
            photonManager = PhotonManager.FindInstance();
        }

        /// <summary>
        /// Instance the prefab to the user, add Animator componet,add properties to the avatar
        /// </summary>
        public override void Spawned()
        {

            var controller = Resources.Load("Animations/Character") as RuntimeAnimatorController;

            //Select Avatar
            if (this.avatar == 0) this.avatar = Random.Range(1, 6);
            GameObject model = Instantiate(playerPrefabs[this.avatar], gameObject.transform.position, gameObject.transform.rotation, gameObject.transform);
            model.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            model.transform.SetAsFirstSibling();

            //Add Animator
            model.AddComponent<Animator>();
            model.GetComponent<Animator>().runtimeAnimatorController = controller;

            //Add Player Tag
            this.gameObject.tag = "Player";

            //Main Player Logic
            if (Object.HasInputAuthority)
            {
                //Set Manager Info
                photonManager.avatarNumber = avatar;
                photonManager.CurrentPlayer = this.gameObject;

                //Initialize NetworkPlayer Info
                playfabIdentity = userManager.UserID;
                Local = this;

                //Enabling Input && Camera
                this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                this.inputHandler.enabled = true;

                //Add LocalPlayer Layer to user
                gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("LocalPlayer");
                foreach (Transform child in gameObject.transform.GetChild(0))
                {
                    child.gameObject.layer = LayerMask.NameToLayer("LocalPlayer");
                }

            }

            else Debug.Log("Spawned remote player");
        }
        /// <summary>
        /// When user left, despawn the Avatar
        /// </summary>
        /// <param name="player"></param>
        public void PlayerLeft(PlayerRef player)
        {
            if (player == Object.InputAuthority) Runner.Despawn(Object);
        }

        /// <summary>
        /// Detects and activates [Networked(OnChanged = nameof(OnNickNameChanged))].
        /// </summary>
        /// <param name="changed"></param>
        static void OnNickNameChanged(Changed<NetworkPlayer> changed)
        {
            Debug.Log($"{Time.time} OnHPChanged value {changed.Behaviour.nickname}");

            changed.Behaviour.OnNickNameChanged();
        }

        /// <summary>
        /// Here we put the name to our local player, we don´t need to do more because our nerworked nickname is also setted so since is networked nickname will be load in every client 
        /// </summary>
        private void OnNickNameChanged()
        {
            Debug.Log($"Nick name changed for player to {nickname} for player {gameObject.name}");

            playerNicknameTM.text = nickname.ToString();


        }


    }
}
