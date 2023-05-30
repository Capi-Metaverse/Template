using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class Multilanguage : MonoBehaviour
{

    void Start()
    {
        Invoke("Delay", 0.1f);
    }

    private void Delay()
    {
        int number = PlayerPrefs.GetInt("Lang");
        Language(number);
    }
    /// <summary>
    /// Take the language from Unity Editor and set the language
    /// </summary>
    /// <param name="lang"></param>
    public void Language(int lang)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[lang];
        PlayerPrefs.SetInt("Lang", lang);
    }
}
