using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using TMPro;
using UnityEngine;

public class PasswordScript : MonoBehaviour
{


    public DoorEvent doorEvent;
    public TMP_InputField inputField;

    public void OpenUI(DoorEvent doorEvent)
    {

        GameObject player = GameManager.FindInstance().GetCurrentPlayer();

        player.GetComponent<CharacterInputHandler>().DeactivateALL();

        this.gameObject.SetActive(true);
        this.doorEvent = doorEvent;
    }

    public void Submit()
    {
        doorEvent.lastPassword = inputField.text;
        doorEvent.activate(true);

    }

    public void Close()
    {
        GameObject player = GameManager.FindInstance().GetCurrentPlayer();
        this.gameObject.SetActive(false);
        player.GetComponent<CharacterInputHandler>().ActiveALL();
    }
}
