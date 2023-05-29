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
        private string chatText = "";
        private string Instruction = @"Act as a random stranger in a chat room and reply to the questions. 
                                    If someone asks you about how to change between rooms, you'll answer  
                                    that they need to go near a door and press the key E. If someone asks  
                                    you about how to interact with an element, reply that they need to go 
                                    near it and press E. If someone asks you about how to mute and unmute 
                                    the voice chat, respond that they need to press the key M. If someone 
                                    asks you about how to open the Settings window, reply that they need to
                                    press Esc. If someone asks you about how to play a presentation, answer 
                                    that they need to go to the map1 or map2 and interact with the yellow block
                                    placed in the Presentation Zone.\nQ: ";

        private void Start()
        {
            button.onClick.AddListener(SendReplyCoroutine);
            inputField.onSubmit.AddListener((string text) => SendReplyCoroutine());
            textArea.text = "You can ask me a question in the input below!";
        }

        private void SendReplyCoroutine()
        {
            StartCoroutine(SendReply());
        }
        /// <summary>
        /// Connect to the Api chatGPT, Send a form and gets the answer from this form and displays it in the text
        /// </summary>
        /// <returns></returns>
        private IEnumerator SendReply()
        {
            userInput = inputField.text;
            Instruction += $"{userInput} \nA: ";

            if (chatText == "")
                chatText = $"Q: {userInput}";
            else
                chatText += $"\n\nQ: {userInput}";

            textArea.text = chatText;

            inputField.text = "";

            button.enabled = false;
            inputField.enabled = false;

            WWWForm form = new WWWForm();
            form.AddField("message", Instruction);

            //Send request
            UnityWebRequest request = UnityWebRequest.Post("https://meta-login.onrender.com/chatgpt", form);
            var handler = request.SendWebRequest();

            //Server connection time
            float startTime = 0.0f;
            while (!handler.isDone)
            {
                if (textArea.text.Length < 3)
                    textArea.text = textArea.text + ".";
                else
                {
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
                string textReceived = request.downloadHandler.text;

                if (textReceived.StartsWith("\nA:"))
                {
                    Instruction += $" {request.downloadHandler.text}";
                    chatText += $" {request.downloadHandler.text}";

                }
                else
                {
                    Instruction += $"\nA: {request.downloadHandler.text}";
                    chatText += $"\nA: {request.downloadHandler.text}";
                }
                textArea.text = chatText;

            }
            //Control de error of the server
            else
            {
                Debug.Log("Unable to connect to the server...");
                textArea.text = "Unable to connect to the server...";
            }

            button.enabled = true;
            inputField.enabled = true;
        }
    }
}