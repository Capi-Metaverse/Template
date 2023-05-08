using Fusion;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;

public class DrawLinesOnPlane : NetworkBehaviour
{
    public Material materialLine;
    public LayerMask planeLayer;
    public Camera camera;
    public Slider sliderGross;
    private float lineWidth = 0.05f;
    private float gross = 1;
    private List<Vector3> linePoints = new List<Vector3>();
    private LineRenderer currentLineRenderer;
    private LineRenderer SendLineRenderer;
    public List<Material> materialsList;
    public GameManager gameManager;
    [SerializeField] private GameObject panelMaterials;

    void Start()
    {
        materialLine = materialsList[0];
        CreateNewLineRenderer();
        gameManager = GameManager.FindInstance();
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

        if (Input.GetMouseButtonUp(0))
        {
            Vector3[] positions = new Vector3[currentLineRenderer.positionCount];


            currentLineRenderer.GetPositions(positions);
            GameManager.RPC_LinesSend(gameManager.GetRunner(), positions);
        }
      
    }

    private void CreateNewLineRenderer()
    {
        GameObject newLineObject = new GameObject("LineRenderer");
        newLineObject.transform.SetParent(transform);
        currentLineRenderer = newLineObject.AddComponent<LineRenderer>();
        currentLineRenderer.material = materialLine;
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
        materialLine= materialsList[1];
    }

    public void ActivatePanelMaterials() 
    {
        if (panelMaterials.activeSelf)
        {
            panelMaterials.SetActive(false);
        }
        else 
        {
            panelMaterials.SetActive(true);
        }
    }
    public void dibujoetc(Vector3[] Lines)
    {
        Debug.Log(Lines.Length);
        //SendLineRenderer.positionCount = Lines.Length;
        GameObject newLineObjectSend = new GameObject("LineRendererSend");
        newLineObjectSend.transform.SetParent(transform);
        SendLineRenderer = newLineObjectSend.AddComponent<LineRenderer>();
        SendLineRenderer.positionCount = Lines.Length;
        SendLineRenderer.SetPositions(Lines);
        SendLineRenderer.material = materialsList[0];
        SendLineRenderer.widthMultiplier = lineWidth * gross;
        SendLineRenderer.useWorldSpace = true;
     
    }


}