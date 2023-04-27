using UnityEngine;

public class TriggerEntranceDoor : MonoBehaviour
{
    [SerializeField] private Animator EntranceDoor = null;

    public bool MemberInside = false;

    float startTime = 0.0f;


    private void OnTriggerStay(Collider other)
    {
        if (MemberInside == false)
        {
            EntranceDoor.Play("OfficeEntranceGlassDoor", 0, 0.0f);
        }
        MemberInside = true;
       

    }

    private void OnTriggerExit(Collider other)
    {
        MemberInside = false;

        startTime = 5.0f;
        while (startTime > 0)
        {

            startTime -= Time.deltaTime;
        }

        if(MemberInside == false)
        {
            EntranceDoor.Play("OfficeEntranceGlassDoorInverse", 0, 0.0f);
        }


    }

}