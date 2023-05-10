using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class LoginUI : MonoBehaviour
{
    TextField email_textfield;
    TextField password_textfield;
    Button forgot_button;
    Button login_button;
    Button register_button;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        email_textfield = root.Q<TextField>("email-input");
        password_textfield = root.Q<TextField>("password-input");
        forgot_button = root.Q<Button>("here-button");
        login_button = root.Q<Button>("login-button");
        register_button = root.Q<Button>("here-button-register");

    }
}
