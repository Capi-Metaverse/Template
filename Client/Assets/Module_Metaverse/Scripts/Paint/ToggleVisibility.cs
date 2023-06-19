using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleVisibility : MonoBehaviour
{
    public GameObject objeto;
    private bool panelVisible;

    private void Start()
    {
        panelVisible = false;
        objeto.SetActive(panelVisible);
    }

    public void TogglePanelVisibility()
    {
        panelVisible = !panelVisible;
        objeto.SetActive(panelVisible);
    }
}