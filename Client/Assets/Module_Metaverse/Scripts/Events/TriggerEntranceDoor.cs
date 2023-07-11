using Fusion;
using UnityEngine;
using Manager;
public class TriggerEntranceDoor : MonoBehaviour
{
    [SerializeField] private Animator EntranceDoor = null;

    public int membersInside = 0;

    GameManager gameManager;

    PhotonManager photonManager;

    private void Start()
    {
        gameManager = GameManager.FindInstance();
        photonManager = PhotonManager.FindInstance();
    }

    /// <summary>
    /// Call GameManger.RPC_OpenDoor to detect if there is more than one user in the area and open the doors.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<NetworkPlayer>().ActorID == photonManager.Runner.LocalPlayer.PlayerId) 
        {
            RPCManager.RPC_OpenDoor(photonManager.Runner);
        }
        
    }

    /// <summary>
    /// Call GameManger.RPC_CloseDoor to detect if there is no user in the area and close the doors.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<NetworkPlayer>().ActorID == photonManager.Runner.LocalPlayer.PlayerId)
        {
           RPCManager.RPC_CloseDoor(photonManager.Runner);
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