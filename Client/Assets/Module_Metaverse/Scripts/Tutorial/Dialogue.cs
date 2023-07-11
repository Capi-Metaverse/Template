
using System.Collections;
using TMPro;
using UnityEngine;

namespace Tutorial
{
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


        /// <summary>
        /// Detects interaction with the Dialogue Window.
        /// </summary>
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

        /// <summary>
        /// Sets the Dialogue Initial Configuration and starts it.
        /// </summary>
        /// <param name="lines"></param>
        public void StartDialogue(string[] lines)
        {
            //Mantener
            EnableDialogue();
            textComponent.text = string.Empty;
            this.lines = lines;
            index = 0;
            StartCoroutine(TypeLine());
        }

        /// <summary>
        /// Writes a line of text in the Dialogue window.
        /// </summary>
        IEnumerator TypeLine()
        {
            foreach (char c in lines[index].ToCharArray())
            {
                textComponent.text += c;
                yield return new WaitForSeconds(textSpeed);
            }
        }

        /// <summary>
        /// Changes to the next line.
        /// </summary>
        void NextLine()
        {
            if (index < lines.Length - 1)
            {
                index++;
                textComponent.text = string.Empty;
                StartCoroutine(TypeLine());
            }
            else DisableDialogue();

        }

        /// <summary>
        /// Activates the dialogue text gameObject.
        /// </summary>
        public void EnableDialogue()
        {
            textDialogue.SetActive(true);
        }

        /// <summary>
        /// Deactivate the dialogue text gameObject.
        /// </summary>
        public void DisableDialogue()
        {
            textDialogue.SetActive(false);
            gameManager.OnInGameStatus();
        }

    }

}
