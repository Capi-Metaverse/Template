using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiUsers : MonoBehaviour ,IMetaEvent
{
    public Dictionary<string, string> inputdata;
    public ManageDataUI manageDataUI;

    public UserUIInfo currentString;
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
            inputdata["Name"] = "Perico";
            inputdata["Teams"] = "Perico@capgemini.com";
            inputdata["About"] = "My name is Peter and i need help";
            inputdata["Hobis"]= "PXT, RTX";

            manageDataUI.data = new UserUIInfo("Perico", "Perico@capgemini.com", "My name is Peter and i need help", "PXT, RTX");
            manageDataUI.SaveData(manageDataUI.data);
        }
        else
        {
            //Assign key values from PlayFab
            inputdata["Name"] = manageDataUI.data.name;
            inputdata["Teams"] = manageDataUI.data.teams;
            inputdata["About"] = manageDataUI.data.about;
            inputdata["Hobis"] = manageDataUI.data.hobbies;

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
    }

    


    

    // Update is called once per frame
    void Update()
    {
        
    }
}
