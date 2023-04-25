
using UnityEngine;
using UnityEngine.UI;

public class UserListItem : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private Button kickButton;
    private int numActor;

    public int NumActor { get => numActor; set => numActor = value; }

    public void Start()
    {
        gameManager = GameManager.FindInstance();

        if (gameManager.GetUserRole() == UserRole.Admin)
        {
            kickButton.gameObject.SetActive(true);
            

        }

        
    }
    public void KickPlayer() { 
        GameManager.RPC_onKick(gameManager.GetRunner(),NumActor);  
    }


  
}