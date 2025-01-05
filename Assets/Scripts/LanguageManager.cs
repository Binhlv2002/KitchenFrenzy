using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance { get; private set; }

    private bool isEnglish; 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        string savedLanguage = PlayerPrefs.GetString("Language", "en");
        isEnglish = savedLanguage == "en";
    }

    public bool IsEnglish()
    {
        return isEnglish;
    }

    public void SetLanguage(bool english)
    {
        isEnglish = english;
        PlayerPrefs.SetString("Language", english ? "en" : "vi");
        PlayerPrefs.Save();
    }

    private void ChangeLanguage(string localeCode)
    {
       
        LanguageManager.Instance.SetLanguage(localeCode == "en");

        Locale locale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);
        LocalizationSettings.SelectedLocale = locale;

        PlayerPrefs.SetString("Language", localeCode);
        PlayerPrefs.Save();
    }

}
