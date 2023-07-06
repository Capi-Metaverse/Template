using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ElevenLabs : MonoBehaviour
{
    private string API_KEY = "e9f1c935dfe5617a03570b39fada8e69";
    //private string VOICE_ID = "TxGEqnHWrfWFTfGW9XjX";

    private AudioSource VOICE_SOURCE = null;

     void Start()
    {
        VOICE_SOURCE = this.gameObject.GetComponent<AudioSource>();
        StartCoroutine(DownloadAudioFromServer());
    }


    IEnumerator DownloadAudioFromServer()
    {

        //string Instruction = "Al habla Adam, vuestro guía en este mundo virtual conocido como Metaverso";

        string jsonString = "{\"text\": \"Hola Peter\"}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);

        UnityWebRequest request = new UnityWebRequest("https://api.elevenlabs.io/v1/text-to-speech/TxGEqnHWrfWFTfGW9XjX", "POST");
        UploadHandlerRaw uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.uploadHandler = uploadHandler;
        request.downloadHandler = new DownloadHandlerAudioClip("https://api.elevenlabs.io/v1/text-to-speech/TxGEqnHWrfWFTfGW9XjX", AudioType.MPEG);
        request.SetRequestHeader("Content-Type", "application/json");

        

        //Send request
       // UnityWebRequest request = UnityWebRequest.Post("https://api.elevenlabs.io/v1/text-to-speech/TxGEqnHWrfWFTfGW9XjX", form);
        request.SetRequestHeader("xi-api-key", API_KEY);
       





        var handler = request.SendWebRequest();

        float startTime = 0.0f;
        while (!handler.isDone)
        {

            startTime += Time.deltaTime;
            if (startTime > 10.0f)
            {
                break;
            }
            yield return null;
        }

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.result.ToString());
            Debug.Log("Error downloading audio: " + request.error);
            yield break;
        }
        // The downloaded audio file will be stored in www.downloadHandler.data

        //Debug.Log(request.downloadHandler.nativeData);

        AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);

        VOICE_SOURCE.clip = audioClip;

        VOICE_SOURCE.Play();





    }
}

