using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace Manager
{
    /// <summary>
    /// Manages the InGame UI
    /// </summary>
    public class UIManager : MonoBehaviour
    {

        [SerializeField] private PlayerUI _playerUI;
        public PlayerUI PlayerUI { get => _playerUI; set => _playerUI = value; }


        [SerializeField] private GameObject _emoteWheel;
        public GameObject EmoteWheel { get => _emoteWheel; set => _emoteWheel = value; }


        [SerializeField] private GameObject _pause;
        public GameObject Pause { get => _pause; set => _pause = value; }


        [SerializeField] private GameObject _settings;
        public GameObject Settings { get => _settings; set => _settings = value; }


        //UI
        [Header("UI")]
        public GameObject UICard;
        public GameObject UICardOtherUser;


        private void Start()
        {
            //Settings
            Settings = GameObject.Find("Menus").transform.GetChild(0).gameObject;

            UICard = GameObject.Find("Menus").transform.GetChild(4).gameObject;
            UICardOtherUser = GameObject.Find("Menus").transform.GetChild(5).gameObject;

        }

        /// <summary>
        /// Static function to get the singleton
        /// </summary>
        /// <returns></returns>
        public static UIManager FindInstance()
        {
            return FindObjectOfType<UIManager>();
        }

        /// <summary>
        /// Activate UI
        /// </summary>
        public void SetUIOn()
        {
            Debug.Log("Activate UI");
            PlayerUI.ShowUI();
        }


        /// <summary>
        /// Deactivate UI
        /// </summary>
        public void SetUIOff()
        {
            Debug.Log("Deactivate UI");
            PlayerUI.HideUI();
        }

        public void HideEventText()
        {
            PlayerUI.EventTextOff();
        }

        public void ShowEventText()
        {
            PlayerUI.EventTextOn();

        }

        public void HidePresentationText()
        {
            PlayerUI.PresentationTextOff();

        }

        public void ShowPresentationText()
        {
            PlayerUI.PresentationTextOn();

        }

        public void OpenPauseMenu()
        {
            if (Pause == null) Pause = GameObject.Find("Menus").transform.GetChild(1).gameObject;
            SetUIOff();
            Pause.SetActive(true);
        }

        public void ClosePauseMenu()
        {
            SetUIOn();
            Pause.SetActive(false);
        }

        public void OpenEmoteWheel()
        {
            if (EmoteWheel == null) EmoteWheel = PhotonManager.FindInstance().CurrentPlayer.transform.GetChild(5).GetChild(0).gameObject;
            Debug.Log(EmoteWheel);
            SetUIOff();
            EmoteWheel.SetActive(true);

        }

        public void CloseEmoteWheel()
        {
            SetUIOn();
            EmoteWheel.SetActive(false);

        }

        /// <summary>
        /// Closes all non Player UIs
        /// </summary>
        public void CloseNonPlayerUI()
        {
            if (EmoteWheel == null) EmoteWheel = PhotonManager.FindInstance().CurrentPlayer.transform.GetChild(5).GetChild(0).gameObject;
            if (Pause == null) Pause = GameObject.Find("Menus").transform.GetChild(1).gameObject;

            SetUIOn();
            EmoteWheel.SetActive(false);
            Pause.SetActive(false);
            Settings.SetActive(false);
            UICard.SetActive(false);
            UICardOtherUser.SetActive(false);

        }




    }
}
