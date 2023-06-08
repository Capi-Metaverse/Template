using UnityEngine;
using TMPro;

public class ImageUpload : IFileUpload
{
    private float _size;
    private Presentation _presentation;
    private TMP_Text _loadingPressCanvas;

    public ImageUpload(float Size, Presentation Presentation, TMP_Text LoadingPressCanvas)
    {
        this._size = Size;
        this._presentation = Presentation;
        this._loadingPressCanvas = LoadingPressCanvas;
    }

    public void FileUpload(byte[] bytes, string fileExtension=null)
    {
        //Create Texture
        Texture2D textu = new Texture2D(1, 1);

        //transform data into texture
        textu.LoadImage(bytes);

        //transform texture into Sprite
        _loadingPressCanvas.enabled = false;
        var nuevoSprite = Sprite.Create(textu, new Rect(0.0f, 0.0f, textu.width, textu.height), new Vector2((float)_size, (float)(_size - 0.07)));
        _presentation.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = nuevoSprite;
    }
}
