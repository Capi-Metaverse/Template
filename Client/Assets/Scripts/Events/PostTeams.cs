using System.Collections;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;


public class PostLAAA : MonoBehaviour
{
    public void Start() 
    {
       StartCoroutine(Prueba()); 
    }

    IEnumerator Prueba() {
        Debug.Log("pruebaaaaaaa");
            var body = new JObject
            {
            ["content"] = "Hola a todos"
            };
            var file = new JArray
            {
            new JObject{
            ["body"] = body
            }
            };
            string jsonString = file.ToString();
                      
            //Send request
            UnityWebRequest request = new UnityWebRequest("https://graph.microsoft.com/v1.0/teams/3816d87f-ab39-4ce9-a9d4-49a96d0b6727/channels/19%3a6Uk-T_8n2qN1HD5HdhqXHHaAsohu_cAtfRHZQ_rCxjY1%40thread.tacv2/messages", "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);
            request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SendWebRequest();
            yield return request;
            Debug.Log(request.downloadHandler.text);
    }   
}
