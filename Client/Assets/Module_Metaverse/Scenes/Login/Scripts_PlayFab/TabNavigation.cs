using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Used for navigate in the UI by pressing the tab
/// </summary>
public class TabNavigation : MonoBehaviour
{
    //Put in the inspector all the components to navigate
    public GameObject[] navigationObjects;

    private int currentIndex = 0;

    //Select the first item of the array
    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(navigationObjects[currentIndex]);
    }

    //Go to the next item of the array when tab is pressed
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentIndex = (currentIndex + 1) % navigationObjects.Length;
            EventSystem.current.SetSelectedGameObject(navigationObjects[currentIndex]);
        }
    }
}
