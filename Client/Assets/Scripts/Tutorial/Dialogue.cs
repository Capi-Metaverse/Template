using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Dialogue : MonoBehaviour
{

    private GameManagerTutorial gameManager;
    public TextMeshProUGUI textComponent;
    public SC_FPSController fpsController;
    public string[] lines;
    public float textSpeed;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Manager").GetComponent<GameManagerTutorial>();
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    public void StartDialogue()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        fpsController.micro.SetActive(false);
        fpsController.scope.SetActive(false);
        fpsController.enabled = false;
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);

        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine (TypeLine());
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            gameObject.SetActive(false);
            gameManager.DialogueStatus = DialogueStatus.InGame;
            fpsController.micro.SetActive(true);
            fpsController.scope.SetActive(true);
            fpsController.enabled = true;
        }
    }
}
