using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    public GameObject eventTextK;
    public Camera presentationCamera = null;
    public bool onPresentationCamera = false;
    public Camera playerCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setPaintCamera(Camera camera)
    {

        //When leaving presentation mode?
        if (camera == null)
        {
            //We deactivate the camera
            presentationCamera = null;

            //We deactivate UI
            eventTextK.SetActive(false);
        }

        else
        {
            //We obtain the camera
            presentationCamera = camera;

            //We activate UI
            eventTextK.SetActive(true);
        }
    }
}
