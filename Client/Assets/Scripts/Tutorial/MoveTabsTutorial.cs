
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using static System.Windows.Forms.LinkLabel;



public class MoveTabsTutorial : MonoBehaviour
{
    [SerializeField] private GameObject TabPanel;
    [SerializeField] private GameObject TabPanelKeys;
    [SerializeField] private GameObject TabPanelPlayers;
    [SerializeField] private GameObject TabPanelFriends;
    [SerializeField] private GameObject TabTabPlayers;
    [SerializeField] private GameObject Lenguages;

    [SerializeField] private Dialogue dialogueScript;

    [SerializeField] private TriggerDetector triggerDetector;

    public GraphicRaycaster graphic;


    [SerializeField] private GameManagerTutorial gameManager;


    private GameObject Settings;
    private GameObject Pause;

    public SettingsStatus settingsStatus = SettingsStatus.None;

    //Maybe improvement in player keys
    public string[] PlayerKeys;

    // Start is called before the first frame update
    void Start()
    {
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

    //Settings Control
    public void HideSettings()
    {
        Settings.SetActive(false);
    }

    public void ShowSettings()
    {
        Settings.SetActive(true);
    }


}