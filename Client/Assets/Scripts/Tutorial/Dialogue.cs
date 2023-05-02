using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum DialogueStatus
{
    InDialogue,
    InGame
}

public class Dialogue : MonoBehaviour
{
    private DialogueStatus dialogueStatus = DialogueStatus.InDialogue;
    public DialogueStatus DialogueStatus { get => dialogueStatus; set => dialogueStatus = value; }

    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    private int index;
    // Start is called before the first frame update
    void Start()
    {
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
            gameObject.SetActive(false);
            dialogueStatus = DialogueStatus.InGame;
        }
    }
}
