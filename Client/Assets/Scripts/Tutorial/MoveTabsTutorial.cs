
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

    /// <summary>
    /// Gets COmponents and activates/deactivates certain settings tabs.
    /// </summary>
    void Start()
    {
        TabTabPlayers.SetActive(true);
        TabPanel.SetActive(true);
        TabPanelKeys.SetActive(false);
        Settings = GameObject.Find("Menus").transform.GetChild(0).gameObject;
        Pause = GameObject.Find("Menus").transform.GetChild(1).gameObject;
        TabPanelPlayers.SetActive(false);
    }

    #region ChangePanels

    /// <summary>
    /// Activates TabPanel and deactivates the rest
    /// </summary>
    public void ChangeToPanel()
    {
        TabPanel.SetActive(true);

        TabPanelKeys.SetActive(false);
        TabPanelPlayers.SetActive(false);
        TabPanelFriends.SetActive(false);
    }

    /// <summary>
    /// Activates TabPanelKeys and deactivates the rest
    /// </summary>
    public void ChangeToPanelKeys()
    {
        TabPanelKeys.SetActive(true);

        TabPanel.SetActive(false);
        TabPanelPlayers.SetActive(false);
        TabPanelFriends.SetActive(false);
    }

    /// <summary>
    /// Activates TabPanelPlayers and deactivates the rest
    /// </summary>
    public void ChangeToPanelPlayer()
    {
        TabPanelPlayers.SetActive(true);

        TabPanel.SetActive(false);
        TabPanelKeys.SetActive(false);
        TabPanelFriends.SetActive(false);
    }

    /// <summary>
    /// Activates TabPanelFriends and deactivates the rest
    /// </summary>
    public void ChangeToPanelFriends()
    {
        TabPanelFriends.SetActive(true);

        TabPanel.SetActive(false);
        TabPanelKeys.SetActive(false);
        TabPanelPlayers.SetActive(false);
    }
    #endregion ChangePanels

    #region SettingsView
    /// <summary>
    /// Deactivates settings and activates the Pause Menu
    /// </summary>
    public void OnClickBackToPause()
    {
        Pause.SetActive(true);
        Settings.SetActive(false);
    }

    /// <summary>
    /// Deactivates Settings Menu
    /// </summary>
    public void HideSettings()
    {
        Settings.SetActive(false);
    }

    /// <summary>
    /// Activates Settings Menu
    /// </summary>
    public void ShowSettings()
    {
        Settings.SetActive(true);
    }
    #endregion SettingsView

}