using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class ImagenData : MonoBehaviour
{
    // Replace with your own title ID
    // Replace with your own title ID

    private string ruta;
    void Start()
    {
        FetchImageData();
    }
    /// <summary>
    /// Open a FileChooser and save the path to convert in image using UploadImagenToUser
    /// </summary>
    public void ClickImageAvatar()
    {
        string path = UnityEditor.EditorUtility.OpenFilePanel("Seleccionar archivo", "", "png,jpg");
        if (!string.IsNullOrEmpty(path))
        {
            ruta = path;
        }

        string imagePath = ruta; // Path to your image file
        byte[] imageData = LoadImageAsByteArray(imagePath);
        UploadImageToUser(imageData);
    }


    /// <summary>
    /// Transform the Path to Bytes
    /// </summary>
    /// <param name="imagePath"></param>
    /// <returns></returns>
    private byte[] LoadImageAsByteArray(string imagePath)
    {
        byte[] imageData = File.ReadAllBytes(imagePath);
        return imageData;
    }
    /// <summary>
    /// PlayFab - Upload the image in playfab in base 64 with Key CustomKey
    /// </summary>
    /// <param name="imageData"></param>
    private void UploadImageToUser(byte[] imageData)
    {
        UpdateUserDataRequest request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "CustomImage", Convert.ToBase64String(imageData) }
            },
            Permission = UserDataPermission.Public
        };

        PlayFabClientAPI.UpdateUserData(request, OnUpdateUserDataSuccess, OnUpdateUserDataFailure);
    }

    private void OnUpdateUserDataSuccess(UpdateUserDataResult result)
    {
        Debug.Log("Image uploaded to user successfully!");

        FetchImageData();
    }

    private void OnUpdateUserDataFailure(PlayFabError error)
    {
        Debug.LogError("Failed to upload image to user: " + error.GenerateErrorReport());
    }


    /// <summary>
    /// PlayFab - Call GetUserData from Playfab
    /// </summary>
    public void FetchImageData()
    {
        GetUserDataRequest request = new GetUserDataRequest();

        PlayFabClientAPI.GetUserData(request, OnGetUserDataSuccess, OnGetUserDataFailure);
    }
    /// <summary>
    /// Data result from FetchImageData, is reconvert to a Image and save like a Sprite, Gets the image component of the object
    /// </summary>
    /// <param name="result"></param>
    private void OnGetUserDataSuccess(GetUserDataResult result)
    {
        if (result.Data != null && result.Data.ContainsKey("CustomImage"))
        {
            string imageString = result.Data["CustomImage"].Value;
            byte[] imageData = System.Convert.FromBase64String(imageString);

            // Use the image data as desired (e.g., display in Unity, save to file, etc.)
            Debug.Log("Image retrieved successfully!");

            Image imagen = GetComponent<Image>();
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            imagen.sprite = sprite;
        }
        else
        {
            Debug.Log("No image data found for the user.");
        }
    }

    private void OnGetUserDataFailure(PlayFabError error)
    {
        Debug.LogError("Failed to fetch user data: " + error.GenerateErrorReport());
    }
}