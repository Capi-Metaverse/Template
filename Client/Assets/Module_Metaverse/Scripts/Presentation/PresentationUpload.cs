using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;


public class PresentationUpload : MonoBehaviour, IFileUpload
{
    private float _size;
    public JObject _json;
    private GameManager _gameManager;
    private Presentation _presentation;

    public PresentationUpload(float _size, Presentation _presentation, GameManager _gameManager)
    {
        this._size = _size;
        this._presentation = _presentation;
        this._gameManager = _gameManager;
    }
    public void FileUpload(byte[] bytes, string fileExtension)
    {

        //Convert data un Base64
        string s = Convert.ToBase64String(bytes);

        //JSON needed to make a post
        var file = new JObject
        {
            ["Name"] = $"file.{fileExtension}",
            ["Data"] = s
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

        StartCoroutine(
            PresentationToImage(
                $"https://v2.convertapi.com/convert/{fileExtension}/to/png?Secret=vCDWYMLnubBCLVDo",
                jsonString));
    }

    IEnumerator PresentationToImage(string PostPath,string jsonString)
    {
        //POST to ConvertApi
        UnityWebRequest request = new UnityWebRequest(PostPath, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();


        _json = JObject.Parse(request.downloadHandler.text);
        Debug.Log("Json: " + _json);

        //TODO: Delete line when status created
        bool PhotonStatus = true;
        if (PhotonStatus == true)
        {
            //We send the content to the other users
            GameManager.RPC_DownloadImages(_gameManager.GetRunner(), request.downloadHandler.text);
        }
        else
        {
            ClearPresentation();
        }

    }

    public void ClearPresentation()
    {
        _presentation.sprites.Clear();
        if (_presentation.current >= 1) _presentation.current = 0;
        if (_presentation.sprites != null) _presentation.sprites.Clear();
        StartCoroutine(ImageRequest());
    }

    /// <summary>
    /// function to make a get request, Get dta from Json with kay Files
    /// </summary>
    /// <returns></returns>
    public IEnumerator ImageRequest()
    {
        foreach (var archiv in _json["Files"])
        {
            Debug.Log(archiv["Url"].ToString());
            //Get Request
            UnityWebRequest GetRequest = UnityWebRequestTexture.GetTexture(archiv["Url"].ToString());
            yield return GetRequest.SendWebRequest();


            switch (GetRequest.result)
            {
                //Error cases
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + GetRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + GetRequest.error);
                    break;

                //Success case
                case UnityWebRequest.Result.Success:
                    //We create a texture2D with the response data
                    Texture2D textu = ((DownloadHandlerTexture)GetRequest.downloadHandler).texture;
                    var nuevoSprite = Sprite.Create(textu, new Rect(0.0f, 0.0f, textu.width, textu.height), new Vector2((float)_size, (float)(_size - 0.07)));
                    _presentation.sprites.Add(nuevoSprite);
                    break;
            }      
        }

        //function in presentation to check if the list is full
        _presentation.OnDirect();
    }
}
