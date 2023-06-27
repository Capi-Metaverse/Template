
using Fusion;
using UnityEngine;





public class GameManager :MonoBehaviour
{

    /*
     * The game manager is the main of the App, it controls all the user connections.
     * 
     * 
     * 
     */

    //Managers

    //Not needed
    //This SceneManager is going to change between scenes and is going to put a loading screen between them.
    [SerializeField] private NetworkSceneManagerBase _loader;

    //PhotonManager
    //Runner, JUST ONE PER USER/ROOM
    //It's like PhotonNetwork.somefunction() in PUN2
    [SerializeField] private NetworkRunner _runner;
    
    //Needed??
    [SerializeField] private InputManager inputManager;

    //PhotonManager
    private GameObject currentPlayer;

    //PhotonManager
    private string roomName; //This is the RoomName
    public int playerCount;

    //SceneManager
    public string currentMap;

    //??
    public GameObject Settings;



    //Static function to get the singleton
    public static GameManager FindInstance()
    {
        return FindObjectOfType<GameManager>();
    }

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

    }

}


