using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tutorial
{
    public class PauseMenuSettingsTutorial : MonoBehaviour
    {
        private GameObject Settings;
        private GameManagerTutorial gameManager;

        [SerializeField] private Dialogue dialogueScript;

        /// <summary>
        /// Gets Components.
        /// </summary>
        private void Start()
        {
            //We find the GameObjects
            gameManager = GameObject.Find("ManagerTutorial").GetComponent<GameManagerTutorial>();
            Settings = GameObject.Find("Menus").transform.GetChild(0).gameObject;

        }

        /// <summary>
        /// If condition is true, it hides the gameobject.
        /// </summary>
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && (gameManager.TutorialStatus != TutorialStatus.Settings) && (gameManager.TutorialStatus != TutorialStatus.PreSettings))
            {
                Hide();
            }
        }

        /// <summary>
        /// Disconnects the User.
        /// </summary>
        public void OnClickDisconnect()
        {
            SceneManager.LoadSceneAsync("1.Start");
        }

        /// <summary>
        /// Activates the gamObject.
        /// </summary>
        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        /// <summary>
        /// Opens the settings menu.
        /// </summary>
        public void OnClickSettings()
        {
            Settings.SetActive(true);
            if (gameManager.TutorialStatus == TutorialStatus.Settings) gameManager.CompleteObjective(0);
            Hide();
        }

        /// <summary>
        /// Deactivates the gameObject.
        /// </summary>
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

    }
}

