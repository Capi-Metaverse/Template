using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class MSceneManager : MonoBehaviour
    {
        [SerializeField] private string _currentScene;
        public string CurrentScene { get => _currentScene; set => _currentScene = value; }


        private void Awake()
        {
            //When this component awake, it get the others game managers
            MSceneManager[] managers = FindObjectsOfType<MSceneManager>();

            //Check if there is more managers
            if (managers != null && managers.Length > 1)
            {
                // There should never be more than a single App container in the context of this sample.
                Destroy(gameObject);
                return;

            }
        }

        //Static function to get the singleton
        public static MSceneManager FindInstance()
        {
            return FindObjectOfType<MSceneManager>();
        }

        //TO DO: Change all maps to enums
        public void LoadLogin()
        {
            CurrentScene = "LoginPlayFab_Module";
            SceneManager.LoadScene(CurrentScene);
        }

        public void LoadMain()
        {
            CurrentScene = "Mapa1";
            SceneManager.LoadScene(CurrentScene);
        }

        public void LoadTutorial()
        {
            CurrentScene = "Tutorial";
            SceneManager.LoadScene(CurrentScene);
        }

        public void LoadScene(string sceneName)
        {
            CurrentScene = sceneName;
            SceneManager.LoadScene(CurrentScene);
        }
    }
}
