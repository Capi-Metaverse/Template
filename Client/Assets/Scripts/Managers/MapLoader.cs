using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MapIndex
{
    Lobby,
    Mapa0,
    Mapa1,
    Mapa2,
    Mapa3,
    HUBValencia,
    Oficinas
};

public class MapLoader : NetworkSceneManagerBase
{
    

    [SerializeField] private GameObject _loadScreen;

    [Header("Scenes")]
    [SerializeField] private SceneReference _lobby;
    [SerializeField] private SceneReference[] _maps;

   

    private void Awake()
    {
        //Put the load screen
        _loadScreen.SetActive(false);
    }

    protected override IEnumerator SwitchScene(SceneRef prevScene, SceneRef newScene, FinishedLoadingDelegate finished)
    {
        Debug.Log($"Switching Scene from {prevScene} to {newScene}");

        _loadScreen.SetActive(true);

        List<NetworkObject> sceneObjects = new List<NetworkObject>();

        string path;
        switch ((MapIndex)(int)newScene)
        {
            case MapIndex.Lobby: path = _lobby; break;
            default: path = _maps[newScene - (int)MapIndex.Mapa0]; break;
        }
        yield return SceneManager.LoadSceneAsync(path, LoadSceneMode.Single);
        var loadedScene = SceneManager.GetSceneByPath(path);

        Debug.Log($"Loaded scene {path}: {loadedScene}");
        sceneObjects = FindNetworkObjects(loadedScene, disable: false);

        // Delay one frame
        yield return null;
        finished(sceneObjects);

        Debug.Log($"Switched Scene from {prevScene} to {newScene} - loaded {sceneObjects.Count} scene objects");

        _loadScreen.SetActive(false);
    }
}
