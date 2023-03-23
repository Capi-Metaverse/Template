using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalCameraHandler : MonoBehaviour
{

    public Transform cameraAnchorPoint;


    Vector2 viewInput;

    float cameraRotationX = 0;
    float cameraRotationY = 0;
    //Other component
    NetworkCharacterControllerPrototypeCustom networkCharacterControllerPrototypeCustom;
    Camera localCamera;
    // Start is called before the first frame update

    private void Awake()
    {
        localCamera = GetComponentInChildren<Camera>();
        networkCharacterControllerPrototypeCustom = GetComponentInParent<NetworkCharacterControllerPrototypeCustom>();
    }
    void Start()
    {
        if (localCamera.enabled)
        {
            localCamera.transform.parent = null;
        }
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(cameraAnchorPoint == null) return;
        
        if (!localCamera.enabled) return;

        localCamera.transform.position = cameraAnchorPoint.position;

        cameraRotationX += viewInput.y * Time.deltaTime * networkCharacterControllerPrototypeCustom.viewUpDownRotationSpeed;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -80, 80);
        cameraRotationY += viewInput.x * Time.deltaTime * networkCharacterControllerPrototypeCustom.viewUpDownRotationSpeed;

        localCamera.transform.localRotation = Quaternion.Euler(cameraRotationX, cameraRotationY, 0);
    }

    public void SetViewInputVector(Vector2 viewInput)
    {
        this.viewInput = viewInput;
    }
}
