using Fusion;
using UnityEngine;

public class TriggerEntranceDoor : MonoBehaviour
{
    [SerializeField] private Animator EntranceDoor = null;

    public int membersInside = 0;

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.FindInstance();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<NetworkPlayer>().ActorID == gameManager.GetRunner().LocalPlayer.PlayerId) 
        {
            GameManager.RPC_OpenDoor(gameManager.GetRunner());
        }
        
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<NetworkPlayer>().ActorID == gameManager.GetRunner().LocalPlayer.PlayerId)
        {
            GameManager.RPC_CloseDoor(gameManager.GetRunner());
        }

    }


    public void OpenDoor()
    {
        EntranceDoor.Play("OfficeEntranceGlassDoor", 0, 0.0f);
    }

    public void CloseDoor()
    {
        EntranceDoor.Play("OfficeEntranceGlassDoorInverse", 0, 0.0f);
    }

}