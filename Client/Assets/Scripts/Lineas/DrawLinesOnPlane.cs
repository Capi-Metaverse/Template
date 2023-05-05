using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class DrawLinesOnPlane : MonoBehaviour
{
    public Material materialLinea;
    public LayerMask planeLayer;
    public Camera camera;
    public Slider sliderGross;
    private float lineWidth = 0.05f;
    private float gross = 1;
    private List<Vector3> linePoints = new List<Vector3>();
    private LineRenderer currentLineRenderer;
    public List<Material> materialsList;


    void Start()
    {
        materialLinea = materialsList[0];
        CreateNewLineRenderer();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Paint"))
            {
                // Perform action on object with "yourTag"
                CreateNewLineRenderer();
            }
        }

        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, float.PositiveInfinity, planeLayer) && hit.transform.tag == "Paint")
            {
                linePoints.Add(hit.point);
            }
        }
        if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) && Input.GetKeyDown(KeyCode.Z))
        {
            int childCount = transform.childCount;
            Debug.Log(childCount);
            if (childCount > 1)
            {
                Transform lastChild = transform.GetChild(childCount - 2);
                Destroy(lastChild.gameObject);
            }

        }

        currentLineRenderer.positionCount = linePoints.Count;
        currentLineRenderer.widthMultiplier = lineWidth * gross;

        Vector3[] nonNullPoints = new Vector3[linePoints.Count];
        int i = 0;
        foreach (Vector3 point in linePoints)
        {
            if (point != null)
            {
                nonNullPoints[i] = point;
                i++;
            }
        }
        currentLineRenderer.SetPositions(nonNullPoints);
    }

    private void CreateNewLineRenderer()
    {
        GameObject newLineObject = new GameObject("LineRenderer");
        newLineObject.transform.SetParent(transform);
        currentLineRenderer = newLineObject.AddComponent<LineRenderer>();
        currentLineRenderer.material = materialLinea;
        currentLineRenderer.widthMultiplier = lineWidth;
        currentLineRenderer.positionCount = 0;
        currentLineRenderer.useWorldSpace = true;
        linePoints.Clear();
    }

    public void Clear()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        CreateNewLineRenderer();
    }

    public void ChangeGross()
    {
        gross = sliderGross.value;
    }

    public void ChangeYellow()
    {
        materialLinea= materialsList[1];
    }




}