using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
   
    GameObject CrossHair;
    GameObject Mic;
    GameObject EventText;
    GameObject EventTextK;

    //Static function to get the singleton
    public static UIManager FindInstance()
    {
        return FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        GameObject currentPlayer = PhotonManager.FindInstance().CurrentPlayer;

        Mic = currentPlayer.transform.GetChild(3).GetChild(0).gameObject;
        CrossHair = currentPlayer.transform.GetChild(3).GetChild(1).gameObject;
        EventText = currentPlayer.transform.GetChild(3).GetChild(2).gameObject;
        EventTextK = currentPlayer.transform.GetChild(3).GetChild(3).gameObject;


    }


    public void SetUIOn()
    {
        //Deactivate presentation text
        EventTextK.SetActive(true);
        EventText.SetActive(true);

        Mic.SetActive(true);
        CrossHair.SetActive(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
       
       
    }

    public void SetUIOff()
    {

        //Deactivate presentation text
        EventTextK.SetActive(false);
        EventText.SetActive(false);

        Mic.SetActive(false);
        CrossHair.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        

    }
}
