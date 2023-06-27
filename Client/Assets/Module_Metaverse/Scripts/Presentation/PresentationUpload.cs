using Newtonsoft.Json.Linq;
using Siccity.GLTFUtility;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class PresentationUpload : MonoBehaviour, IFileUpload
{
    public float _size;
    public JObject _json;
    public GameManager _gameManager;
    public Presentation _presentation;

    public void Start()
    {
        _presentation = GameObject.Find("Presentation").GetComponent<Presentation>();
        _gameManager = GameManager.FindInstance();
    }

    public void FileUpload(byte[] bytes, string fileExtension)
    {
        string base64Data = System.Convert.ToBase64String(bytes);

        var file = new JObject
        {
            ["Name"] = $"file.{fileExtension}",
            ["Data"] = base64Data
        };

        var parameters = new JArray
        {
            new JObject
            {
                ["Name"] = "file",
                ["FileValue"] = file
            },
            new JObject
            {
                ["Name"] = "StoreFile",
                ["Value"] = true
            }
        };

        var jsonObject = new JObject
        {
            ["Parameters"] = parameters
        };

        string jsonString = jsonObject.ToString();
        Debug.Log(jsonString);

        StartCoroutine(PresentationToImage($"https://v2.convertapi.com/convert/{fileExtension}/to/png?Secret=vCDWYMLnubBCLVDo", jsonString));
    }

    IEnumerator PresentationToImage(string postPath, string jsonString)
    {
        UnityWebRequest request = new UnityWebRequest(postPath, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        _json = JObject.Parse(request.downloadHandler.text);
        Debug.Log("Json: " + _json);

        bool photonStatus = true;
        if (photonStatus)
        {
            RPCManager.RPC_DownloadImages(_gameManager.GetRunner(), request.downloadHandler.text);
        }
        else
        {
            ClearPresentation();
        }
    }

    public void ClearPresentation()
    {
        _presentation.transform.GetChild(2).gameObject.SetActive(false);
        _presentation.sprites.Clear();
        if (_presentation.current >= 1) _presentation.current = 0;
        if (_presentation.sprites != null) _presentation.sprites.Clear();
        StartCoroutine(ImageRequest());
    }

    public IEnumerator ImageRequest()
    {
        List<UnityWebRequest> webRequests = new List<UnityWebRequest>();

        foreach (var archive in _json["Files"])
        {
            string url = archive["Url"].ToString();
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            webRequests.Add(request);
            request.SendWebRequest();
        }

        foreach (UnityWebRequest request in webRequests)
        {
            while (!request.isDone)
            {
                yield return null;
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + request.error);
                continue;
            }

            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            var newSprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(_size, _size - 0.07f));
            _presentation.sprites.Add(newSprite);
        }

        _presentation.OnDirect();
    }
}
