using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintTool : MonoBehaviour
{
    public Material paintBrushMaterial;
    public Camera mainCamera;

    public float brushSize = 1.0f;
    public Color brushColor = Color.red;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = mainCamera.ScreenPointToRay(mousePos);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                Paint(hitInfo.point);
            }
        }
    }

    void Paint(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(position, Vector3.forward, out hit))
        {
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = paintBrushMaterial;
                renderer.material.color = brushColor;
            }
        }
    }
}