using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;


namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {
        [SerializeField] private InputField inputField;
        [SerializeField] private Button button;
        [SerializeField] private Text textArea;

        private string userInput;
        private string Instruction = "Act as a random stranger in a chat room and reply to the questions. If someone asks you about how to change between rooms, you'll answer that they need to go near a door and press the key E \nQ: ";

        private void Start()
        {
            button.onClick.AddListener(SendReplyCoroutine);
            inputField.onSubmit.AddListener((string text) => SendReplyCoroutine());
            textArea.text = "You can ask me a question in the input below!";
        }

        private void SendReplyCoroutine(){
            StartCoroutine(SendReply());
        }

        private IEnumerator SendReply()
        {
            userInput = inputField.text;
            Instruction += $"{userInput} \nA: ";
            
            inputField.text = "";

            button.enabled = false;
            inputField.enabled = false;

            WWWForm form = new WWWForm();
            form.AddField("message", Instruction);

            //Send request
            UnityWebRequest request = UnityWebRequest.Post("https://meta-login.onrender.com/chatgpt",form);
            var handler = request.SendWebRequest();

            //Server connection time
            float startTime = 0.0f;
            while (!handler.isDone)
            {
                if(textArea.text.Length < 3)
                textArea.text = textArea.text + ".";
                else{
                    textArea.text = ".";
                }

                startTime += Time.deltaTime;
                if (startTime > 10.0f)
                {
                    break;
                }
                yield return null;
            }

            //Control the server response
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(request.downloadHandler.text);
            }
            //Control de error of the server
            else
            {
                Debug.Log("Unable to connect to the server...");
                textArea.text = "Unable to connect to the server...";
            }


            textArea.text = request.downloadHandler.text;
            Instruction += $"{request.downloadHandler.text}\nQ: ";

            button.enabled = true;
            inputField.enabled = true;
        }
    }
}



