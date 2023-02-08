using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTabs : MonoBehaviour
{
    public GameObject TabPanel;
    public GameObject TabPanelKeys;
    // Start is called before the first frame update
    void Start()
    {
        TabPanel.SetActive(true);
        TabPanelKeys.SetActive(false);
    }
    public void ChangeToPanel(){
        TabPanel.SetActive(true);
        TabPanelKeys.SetActive(false);
    }

    public void ChangeToPanelKeys(){
        TabPanel.SetActive(false);
        TabPanelKeys.SetActive(true);
    }
}
