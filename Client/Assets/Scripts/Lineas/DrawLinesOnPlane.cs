using System.Collections.Generic;
using UnityEngine;

public class DrawLinesOnPlane : MonoBehaviour
{
    public Material lineMaterial;
    private float lineWidth = 0.1f;
    public LayerMask planeLayer;
    private List<Vector3> linePoints = new List<Vector3>();
    private LineRenderer lineRenderer;
    public Camera Camera;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.widthMultiplier = lineWidth ;
        lineRenderer.positionCount = 0;
        lineRenderer.useWorldSpace = true;
    }
 
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            
            if (Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out hit, float.PositiveInfinity, planeLayer) && hit.transform.tag == "Paint")
            {
                linePoints.Add(hit.point);
            }
        }
        if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) && Input.GetKey(KeyCode.Z))
        {
            if (linePoints.Count > 0)
            {
                linePoints.RemoveAt(linePoints.Count - 1);
            }
        }

        lineRenderer.positionCount = linePoints.Count;
        lineRenderer.widthMultiplier = lineWidth;
        lineRenderer.SetPositions(linePoints.ToArray());

    }
}