using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Dialogue : MonoBehaviour
{

    //Variables
    //Change public to serializable
   
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private SC_FPSController fpsController;
    [SerializeField] private MoveTabsTutorial moveTabs;
    [SerializeField] private Animator animator;
    [SerializeField] private TMP_Text reShowText;
    [SerializeField] private string[] lines;
    [SerializeField] private float textSpeed;
   

    [SerializeField] private GameObject textDialogue;

    [SerializeField] private GameManagerTutorial gameManager;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        
        //StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        //If user clicks, and it's not ended then this code completes the line.
        if (Input.GetMouseButtonDown(0) && gameManager.DialogueStatus == DialogueStatus.InDialogue)
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

    public void StartDialogue(string[] lines)
    {
        //Mantener
        EnableDialogue();
        textComponent.text = string.Empty;
        this.lines = lines;
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
            DisableDialogue();
            /*
            //Cambiar
            if (gameManager.TutorialStatus != TutorialStatus.Settings)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                DisableDialogue();
                gameManager.DialogueStatus = DialogueStatus.InGame;
                fpsController.playerUI.ShowUI();
                fpsController.enabled = true;
                reShowText.enabled = true;
                animator.SetFloat("Speed", 1);
            }
            else
            {
                DisableDialogue();
                if (moveTabs.settingsStatus >= SettingsStatus.Settings)
                moveTabs.NextTutorial();
            }

            */


            
        }
    }

    public void EnableDialogue()
    {
        textDialogue.SetActive(true);
    }

    public void DisableDialogue()
    {
        textDialogue.SetActive(false);
        gameManager.OnInGameStatus();
    }

}
