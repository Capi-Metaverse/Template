
using UnityEngine;
public class GameManager :MonoBehaviour
{

    /*
     * The game manager is the main of the App, it controls all the user connections.
     * 
     * 
     * 
     */

    //Needed??
    [SerializeField] private InputManager inputManager;

    //??
    public GameObject Settings;
    public float Sensitivity;


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


