using UnityEngine;

public class TriggerEntranceDoor : MonoBehaviour
{
    [SerializeField] private Animator EntranceDoor = null;

    public bool MemberInside { get; set; } = false;

    float startTime = 0.0f;

    private void OnTriggerStay(Collider other)
    {
        if (MemberInside == false)
        {
            EntranceDoor.Play("OfficeEntranceGlassDoor", 0, 0.0f);
        }

        MemberInside = true;

        startTime = 5;
        while (startTime>0)
        {
            startTime -= Time.deltaTime;
        }

        MemberInside = false;
        EntranceDoor.Play("OfficeEntranceGlassDoorInverse", 0, 0.0f);

    }

}