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
    public MoveTabsTutorial moveTabs;
    public Animator animator;
    public TMP_Text reShowText;
    public string[] lines;
    public float textSpeed;
    private int index;

    [SerializeField] private GameObject textDialogue;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("ManagerTutorial").GetComponent<GameManagerTutorial>();
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        //If user clicks, and it's not ended then this code completes the line.
        if (Input.GetMouseButtonDown(0) && gameManager.DialogueStatus== DialogueStatus.InDialogue)
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
        animator.SetFloat("Speed", 0);
        gameManager.DialogueStatus = DialogueStatus.InDialogue;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        fpsController.playerUI.HideUI();
        fpsController.enabled = false;
        reShowText.enabled = false;
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
            if (gameManager.TutorialStatus != TutorialStatus.Settings)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                DisableDialogue();
                gameManager.DialogueStatus = DialogueStatus.InGame;
                fpsController.playerUI.ShowUI();
                fpsController.enabled = true;
                Debug.Log("aaaa");
                reShowText.enabled = true;
                animator.SetFloat("Speed", 1);
            }
            else
            {
                DisableDialogue();
                if (moveTabs.settingsStatus >= SettingsStatus.Settings)
                moveTabs.NextTutorial();
            }
        }
    }

    public void EnableDialogue()
    {
        textDialogue.SetActive(true);
    }

    public void DisableDialogue()
    {
        textDialogue.SetActive(false);
    }
}
