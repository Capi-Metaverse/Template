using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UiUsers : MonoBehaviour ,IMetaEvent
{
    public Dictionary<string, string> inputdata;
    public ManageDataUI manageDataUI;
    public GameObject UICard;
    public VisionData visionData;
    public UserUIInfo currentString;
    GameManager gameManager;
    GameObject scope;
    // Start is called before the first frame update
   
    IEnumerator LoadInitialData()
    {
        // Do some work here...
        yield return null; // Wait one frame

        // Do some more work here...
        yield return new WaitForSeconds(4); // Wait for 4 seconds

        inputdata = new Dictionary<string, string>();
        if (manageDataUI.data == null)
        {
            inputdata["Name"] = gameManager.GetUsername();
            inputdata["Teams"] = gameManager.GetEmail();
            inputdata["About"] = "Por defecto";
            inputdata["Hobis"]= "Por defecto";
            inputdata["CV"] = "Por defecto";

            manageDataUI.data = new UserUIInfo(gameManager.GetUsername(), gameManager.GetEmail(), "Por defecto", "Por defecto", "Por defecto");
            manageDataUI.SaveData(manageDataUI.data);
        }
        else
        {
            //Assign key values from PlayFab
            inputdata["Name"] = gameManager.GetUsername();
            inputdata["Teams"] = gameManager.GetEmail(); ;
            inputdata["About"] = manageDataUI.data.about;
            inputdata["Hobis"] = manageDataUI.data.hobbies;
            inputdata["CV"] = manageDataUI.data.CV;

            visionData.UserNameTitle.text = gameManager.GetUsername();
            visionData.TemasText.text = gameManager.GetEmail(); ;
            visionData.OboutText.text = manageDataUI.data.about;
            visionData.HobbiesText.text = manageDataUI.data.hobbies;
            visionData.CVText.text = manageDataUI.data.CV;

        }
        currentString = manageDataUI.data;
        // Function is finished
    }
    // Start is called before the first frame update
    public void activate(bool host)
    {
        manageDataUI = new ManageDataUI();
        manageDataUI.LoadData();
        StartCoroutine(LoadInitialData());

        UICard.SetActive(true);
        
        
    }
    void Start()
    {
        gameManager = GameManager.FindInstance();
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
