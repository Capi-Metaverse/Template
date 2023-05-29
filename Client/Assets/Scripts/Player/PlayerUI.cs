using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Ui from users
/// </summary>
public class PlayerUI : MonoBehaviour
{

    //Private

    [SerializeField] private GameObject mic;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject eventText;
    [SerializeField] private GameObject presentationText;


    //Mic Sprites
    private Sprite microOn, microOff;
    private bool isMicOn = false;


    //Public

    public GameObject Mic { get => mic; set => mic = value; }
    public GameObject Crosshair { get => crosshair; set => crosshair = value; }
    public GameObject EventText { get => eventText; set => eventText = value; }
    public GameObject PresentationText { get => presentationText; set => presentationText = value; }


    


    public void Start()
    {
        microOff = Resources.Load<Sprite>("Sprites/UI/micro_off");
        microOn = Resources.Load<Sprite>("Sprites/UI/micro_on");

        mic.GetComponent<Image>().sprite = microOff;

    }



    public void MicOff()
    {
        mic.SetActive(false);
       
    
    }

    public void MicOn()
    {
        mic.SetActive(true);
       
    }

    public void ChangeMicSprite()
    {
        if (isMicOn)
        {
         
            mic.GetComponent<Image>().sprite = microOff;
        }

        else
        {
            mic.GetComponent<Image>().sprite = microOn;
        }

        isMicOn = !isMicOn;
    }


    public void CrosshairOff()
    {
        crosshair.SetActive(false);
    }

    public void CrosshairOn()
    {
        crosshair.SetActive(true);
    }

    public void EventTextOff()
    {
        eventText.SetActive(false);
    }

    public void EventTextOn()
    {
        eventText.SetActive(true);
    }

    public void PresentationTextOff()
    {
        presentationText.SetActive(false);
    }
    public void PresentationTextOn()
    {
        presentationText.SetActive(true);
    }

    public void HideUI()
    {
        MicOff();
        CrosshairOff();
        EventTextOff();
        PresentationTextOff();
    }

    public void ShowUI()
    {
        MicOn();
        CrosshairOn();
    
    }

}
