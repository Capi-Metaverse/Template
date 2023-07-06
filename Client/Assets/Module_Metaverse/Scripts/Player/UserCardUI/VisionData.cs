using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Interface to CV 
/// </summary>
public class VisionData : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text UserNameTitle;
    public TMP_Text TemasText;
    public TMP_Text OboutText;
    public TMP_Text HobbiesText;
    public TMP_Text CVText;

    public GameObject OnlyVisionData;
    public GameObject EditVisionDat;
    public void changeEditData()
    {
        EditVisionDat.SetActive(true);
        OnlyVisionData.SetActive(false);
    }

}

