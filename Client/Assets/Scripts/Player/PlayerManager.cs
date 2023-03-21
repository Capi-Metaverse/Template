
using Fusion;
using UnityEngine;


public class PlayerManager : NetworkBehaviour
{


    /*------------MANAGERS AS GAMEOBJECTS IN SCENE MAP----------------*/

    public Animator animator;//Animation manager
    public GameObject Pausa;//Pausa is an object in scene map, you can see it as the manager of the pause state
    public GameObject chatManager;//Also in scene map is the manager for the TextChat(T to open TextChat)
    public GameObject Settings;//The same as Pausa but for settings, the state will be Pausa too cause the setting are accesible from Pausa

    /*--------------------BOOLEANS FOR CONDITIONAL CHECKING---------------------*/
    public Estados estado; //With this we keep track of the current state so we can use it in conditionals. States are (Game, Pause)
    public bool onPresentationCamera = false;//Boolean to know if pressMode is on or not
    bool escPul;//Reference if ESC key is pushed or not(ESC opens the Menu and you´ll be on Pause State)
    bool TPul;//Reference if T key is pushed or not(T opens the TextChat)

    /*-------------UTILITY VARIABLES-----------------*/
    public GameObject[] playerPrefabs;//This are the player models(ReadyPlayerMe), all of them in a list
    public Camera presentationCamera = null;//Outside of the PlayerPrefab. With K you can change from playerCam to this cam that look ortographically to the presentation
    public GameObject playerCamera;//Inside of the PlayerPrefab
    public GameObject playerToSpawn;//Player Character
    public Transform[] spawnPoints;//Array with points corresponding to points in the map, the players spawn randomly
    static Vector3 spawnPoint;//A 3 coordinate point where the player must spawn
    public GameObject scope;//Actually this is the crosshair in game
    public GameObject micro;//Actually this is the microphone in game
    public GameObject eventTextK;//A text for displaying when an event happens
    GameObject canvasGPT;

    //Fusion

    [Networked]
    private int avatar { get; set; }

    public enum Estados
    {
        Juego, Pausa
    }

    /*-------------METHODS-----------------*/

    public static void SpawnPlayer(NetworkRunner runner, PlayerManager player)
    {
        Debug.Log("SpawningPlayer");
        Debug.Log(player);

        Debug.Log(runner.State);
        runner.Spawn(player,
        Vector3.zero,
        Quaternion.identity,
        inputAuthority: runner.LocalPlayer,
        BeforeSpawnPlayer,
        predictionKey: null);
        

    }

    public override void Spawned()
    {
        base.Spawned();
     

    }



    public static void BeforeSpawnPlayer(NetworkRunner runner, NetworkObject obj)
    {

        //Calculate the model
        PlayerManager player = obj.GetComponent<PlayerManager>();
        player.avatar = GameManager.FindInstance().avatarNumber;

        /*
        int randomNumber = Random.Range(0, spawnPoints.Length);
        spawnPoint = spawnPoints[randomNumber].position;

        */

        if (player.avatar == 0) player.avatar = Random.Range(1, 6);

        //We set the model as a child
        GameObject[] playerPrefabs = GameManager.FindInstance().playerPrefabs;

  
        GameObject model = Instantiate(playerPrefabs[player.avatar]);
        model.transform.SetParent(obj.transform);


        obj.GetComponent<NetworkCharacterControllerPrototype>().InterpolationTarget = obj.transform.GetChild(0);



    }
}
