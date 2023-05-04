using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialStatus
{
    Movement,
    Jumping,
    Interaction,
    Voice,
    Settings,
    Presentation,
    Animations,
    Finished
}

public enum DialogueStatus
{
    InDialogue,
    InGame
}

public enum GameStatus
{
    InPause,
    InGame,
}

public class GameManagerTutorial : MonoBehaviour
{
    //TutorialStatus
    private TutorialStatus tutorialStatus = TutorialStatus.Movement;
    public TutorialStatus TutorialStatus { get => tutorialStatus; set => tutorialStatus = value; }

    //DialogueStatus
    private DialogueStatus dialogueStatus = DialogueStatus.InDialogue;
    public DialogueStatus DialogueStatus { get => dialogueStatus; set => dialogueStatus = value; }

    //GameStatus
    private GameStatus gameStatus = GameStatus.InGame;
    public GameStatus GameStatus { get => gameStatus; set => gameStatus = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
