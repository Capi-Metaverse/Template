
using UnityEngine;

using Manager;
public class MoveTabs : MonoBehaviour 
{
    [SerializeField] private GameObject TabPanel;
    [SerializeField] private GameObject TabPanelKeys;
    [SerializeField] private GameObject TabPanelPlayers;
    [SerializeField] private GameObject TabPanelFriends;
    [SerializeField] private GameObject TabTabPlayers;
    [SerializeField] private GameObject Lenguages;

    
    private GameObject Settings;
    private GameObject Pause;

    //Maybe improvement in player keys
    public string[] PlayerKeys;

    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Manager").GetComponent<GameManager>();

        TabTabPlayers.SetActive(true);
        TabPanel.SetActive(true);
        TabPanelKeys.SetActive(false);
        Settings = GameObject.Find("Menus").transform.GetChild(0).gameObject;
        Pause = GameObject.Find("Menus").transform.GetChild(1).gameObject;
        TabPanelPlayers.SetActive(false);
    }
    public void ChangeToPanel()
    {
        TabPanel.SetActive(true);
        TabPanelKeys.SetActive(false);
        TabPanelPlayers.SetActive(false);
        TabPanelFriends.SetActive(false);
    }

    public void ChangeToPanelKeys()
    {
        TabPanel.SetActive(false);
      
        TabPanelKeys.SetActive(true);
        TabPanelPlayers.SetActive(false);
        TabPanelFriends.SetActive(false);
    }
    public void ChangeToPanelPlayer()
    {

        //We initialize the player list
    

        TabPanel.SetActive(false);
      
        TabPanelKeys.SetActive(false);
        TabPanelPlayers.SetActive(true);
        TabPanelFriends.SetActive(false);
    }
    public void ChangeToPanelFriends()
    {
        

        
        TabPanel.SetActive(false);
       
        TabPanelKeys.SetActive(false);
        TabPanelPlayers.SetActive(false);
        TabPanelFriends.SetActive(true);
        
    }

    public void OnClickBackToPause()
    {
        Pause.SetActive(true);
        Settings.SetActive(false);
    }

}