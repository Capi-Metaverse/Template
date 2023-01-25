using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerNameDisplay : MonoBehaviour
{
    public Camera playerCamera; // Assign the player's camera in the inspector
    public TMP_Text nameText; // Assign the Text element in the inspector

    // Update is called once per frame
    void Update()
    {
        // Rotate the canvas to face the player camera
        //nameText.transform.LookAt(nameText.transform.position + playerCamera.transform.rotation * Vector3.forward, playerCamera.transform.rotation * Vector3.up);
    }

    public void SetPlayerName(string name)
    {
        // Update the text element with the player's name
        nameText.text = name;
    }
}

