using System.Collections;

using UnityEngine;

public class UiUsers : MonoBehaviour ,IMetaEvent
{
   
    public ManageDataUI manageDataUI;
    public AchievementsManager achievementsManager;
    public GameObject UICard;
    public VisionData visionData;
    public EditVisionData editVisionData;
    public UserUIInfo currentString;
    public AchievementList achivementList;
   
    
    UserManager userManager;
    CharacterInputHandler characterInputHandler;

    GameObject _eventObject;
    GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }
    // Start is called before the first frame update
    /// <summary>
    /// if there is no data in playfab, it sets default data, if it does not get it, it will request it.
    /// </summary>
    /// <returns></returns>
    public IEnumerator LoadInitialData()
    {
        // Do some work here...
        yield return null; // Wait one frame

        // Do some more work here...
        yield return new WaitForSeconds(1); // Wait for 1 seconds


        userManager = UserManager.FindInstance();
     
        if (manageDataUI.data == null)
        {
            manageDataUI.data = new UserUIInfo(userManager.Username, userManager.Email, "Por defecto", "Por defecto", "Por defecto");
           
            manageDataUI.SaveData(manageDataUI.data);
        }
      
        editVisionData.UserNameTitle.text = userManager.Username;
        editVisionData.TemasText.text= userManager.Email;

        visionData.UserNameTitle.text = userManager.Username;
        visionData.TemasText.text = userManager.Email; ;
        visionData.OboutText.text = manageDataUI.data.about;
        visionData.HobbiesText.text = manageDataUI.data.hobbies;
        visionData.CVText.text = manageDataUI.data.CV;
        currentString = manageDataUI.data;


        achivementList.InstanceAchievementItem();
        // Function is finished
    }
    // Start is called before the first frame update
    /// <summary>
    /// Detects activation, e.g. when the chair is clicked, initialises the components and calls functions to get the information from playfab, also calls LoadInitialData.
    /// </summary>
    /// <param name="host"></param>
    public void activate(bool host)
    {
        characterInputHandler = PhotonManager.FindInstance().CurrentPlayer.gameObject.GetComponent<CharacterInputHandler>();
        UIManager uiManager = UIManager.FindInstance();
        PauseManager pauseManager = PauseManager.FindInstance();
        characterInputHandler.EnableMovement = false;
        pauseManager.Pause();
        uiManager.SetUIOff();
        
        manageDataUI = new ManageDataUI();
        manageDataUI.LoadData();
        achievementsManager.LoadData();
        StartCoroutine(LoadInitialData());

        UICard.SetActive(true);

       
        //characterInputHandler.DeactivateALL();
    }
   
    // Update is called once per frame
    /// <summary>
    /// change the playfab values to the values you set in the inputs
    /// </summary>
    public void EditUserData()
    {
        manageDataUI.data = new UserUIInfo(userManager.Username, userManager.Email, editVisionData.OboutTextInput.text, editVisionData.HobbiesTextInput.text, editVisionData.CVTextInput.text);
        manageDataUI.SaveData(manageDataUI.data);
    }
}
