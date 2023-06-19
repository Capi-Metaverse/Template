using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class ColorPicker : MonoBehaviour
{
    [SerializeField] RectTransform _texture;
    [SerializeField] DrawLinesOnPlane _drawLinesOnPlane;
    [SerializeField] Texture2D _refSprite;
    [SerializeField] Material _materialColorPicker;



    private bool mouseIn = false;
    private Color _color;


    public void OnClickPickerColor()
    {
        SetColor();
    }



    private void SetColor()
    {
        if (mouseIn)
        {
            Vector3 imagePos = _texture.position;
            float globalPosX = Input.mousePosition.x - imagePos.x;
            float globalPosY = Input.mousePosition.y - imagePos.y;



            int localPosX = (int)(globalPosX * (_refSprite.width / _texture.rect.width));
            int localPosY = (int)(globalPosY * (_refSprite.height / _texture.rect.height));



            _color = _refSprite.GetPixel(localPosX, localPosY);
            _materialColorPicker.color = _color;
        }
    }


    public void MouseIn()
    {
        mouseIn = true;
        Debug.Log("IN");
    }



    public void MouseOut()
    {
        mouseIn = false;
        Debug.Log("OUT");
    }



    public void OnPointerUp()
    {
        _drawLinesOnPlane.CreateNewMaterial(_color);
    }
}