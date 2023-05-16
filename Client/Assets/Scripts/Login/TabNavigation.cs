using UnityEngine;
using UnityEngine.EventSystems;

public class TabNavigation : MonoBehaviour
{
    public GameObject[] navigationObjects;

    private int currentIndex = 0;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(navigationObjects[currentIndex]);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentIndex = (currentIndex + 1) % navigationObjects.Length;
            EventSystem.current.SetSelectedGameObject(navigationObjects[currentIndex]);
        }
    }
}
