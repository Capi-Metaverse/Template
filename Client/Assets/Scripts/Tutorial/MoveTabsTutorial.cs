
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

    /*
    public void StartTutorial()
    {
        //Dialogo settings

        graphic.enabled = false;

        string[] lines = new string[1] { "This is the settings menu. You can change some options like Volume or Sensivity." };
        dialogueScript.lines = lines;
        dialogueScript.textComponent.text = string.Empty;
        dialogueScript.transform.GetChild(0).gameObject.SetActive(true);
        settingsStatus = SettingsStatus.Settings;
        dialogueScript.StartDialogue();
        //Mover tab
    }
    */

    //public void NextTutorial()
    //{
    //    settingsStatus++;

    //    switch (settingsStatus)
    //    {
    //        case SettingsStatus.Keys:
    //            {
    //                ChangeToPanelKeys();

    //                string[] lines = new string[1] { "This is the Key menu. You can change the input keys from here." };
    //                dialogueScript.lines = lines;
    //                dialogueScript.textComponent.text = string.Empty;
    //                dialogueScript.transform.GetChild(0).gameObject.SetActive(true);

    //                dialogueScript.StartDialogue();
    //                break;
    //            }

    //        case SettingsStatus.Friends:
    //            {
    //                ChangeToPanelFriends();

    //                string[] lines = new string[1] { "This is the Friends menu. You can see the friends that you add here." };
    //                dialogueScript.lines = lines;
    //                dialogueScript.textComponent.text = string.Empty;
    //                dialogueScript.transform.GetChild(0).gameObject.SetActive(true);

    //                dialogueScript.StartDialogue();
    //                break;
    //            }

    //        case SettingsStatus.Players:
    //            {
    //                ChangeToPanelPlayer();
    //                string[] lines = new string[1] { "This is the Player menu. You can see the list of players here." };
    //                dialogueScript.lines = lines;
    //                dialogueScript.textComponent.text = string.Empty;
    //                dialogueScript.transform.GetChild(0).gameObject.SetActive(true);

    //                dialogueScript.StartDialogue();
    //                break;
    //            }

    //        case SettingsStatus.Finished:
    //            {
                   
    //                Settings.SetActive(false);


    //                string[] lines = new string[2] { "That's all about the settings part.", "Now, go downstairs and interact with the podium to view a presentation" };
    //                dialogueScript.lines = lines;
    //                dialogueScript.textComponent.text = string.Empty;
    //                dialogueScript.transform.GetChild(0).gameObject.SetActive(true);

    //                dialogueScript.StartDialogue();

    //                gameManager.GameStatus = GameStatus.InGame;
    //                gameManager.TutorialStatus = TutorialStatus.Presentation;
    //                graphic.enabled = true;

    //                triggerDetector.SetPresentationTutorial();
                   

    //                break;
    //            }
    //    }
    //}
      
  

}