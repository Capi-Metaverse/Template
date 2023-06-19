using TMPro;
using UnityEngine;

public class MapArrows : MonoBehaviour, ICreateRoomArrows
{
    [SerializeField] private TMP_Text map;

    private string[] mapNames = { "Mapa1", "Mapa2", "Oficinas" };

    private int actualMap = 0;

    /// <summary>
    /// Selecto previous map
    /// </summary>
    public void OnLeftClick()
    {
        if (actualMap > 0)
        {
            map.text = mapNames[--actualMap];
        }
    }

    /// <summary>
    /// Select next map
    /// </summary>
    public void OnRightClick()
    {
        if (actualMap < mapNames.Length - 1)
        {
            map.text = mapNames[++actualMap];
        }
    }
}
