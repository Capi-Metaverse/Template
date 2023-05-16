using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EditVisionData : MonoBehaviour
{
    public ImagenData imagedata;

    public TMP_Text UserNameTitle;
    public TMP_Text TemasText;
    public TMP_InputField OboutTextInput;
    public TMP_InputField HobbiesTextInput;
    public TMP_InputField CVTextInput;
    public UiUsers uiusers;

    public GameObject OnlyVisionData;
    public GameObject EditVisionDat;

    public void changeViewDataAndSave()
    {
        uiusers.EditUserData();
        imagedata.FetchImageData();
        StartCoroutine(uiusers.LoadInitialData());
        EditVisionDat.SetActive(false);
        OnlyVisionData.SetActive(true);
        
    }
}