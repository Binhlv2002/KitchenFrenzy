using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playMultiplayerButton;
    [SerializeField] private Button playSingleplayerButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button languageButton;

    [SerializeField] private Sprite englishFlag;
    [SerializeField] private Sprite vietnameseFlag;

    private void Awake()
    {
        playSingleplayerButton.onClick.AddListener(() =>
        {
            GameMultiplayer.playMultiplayer = false;
            Loader.Load(Loader.Scene.LobbyScene);


        });

        playMultiplayerButton.onClick.AddListener(() =>
        {
            GameMultiplayer.playMultiplayer = true;
            Loader.Load(Loader.Scene.LobbyScene);
        });

        quitButton.onClick.AddListener(Application.Quit);

        languageButton.onClick.AddListener(ToggleLanguage);

        Time.timeScale = 1f;
    }


    private void Start()
    {

        string savedLanguage = PlayerPrefs.GetString("Language", "en");
        ChangeLanguage(savedLanguage);
    }

    private void ToggleLanguage()
    {
    
        string newLanguage = GetCurrentLanguage() == "en" ? "vi" : "en";
        ChangeLanguage(newLanguage);
    }

    private void ChangeLanguage(string localeCode)
    {
        
        Locale locale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);
        LocalizationSettings.SelectedLocale = locale;

        
        PlayerPrefs.SetString("Language", localeCode);
        PlayerPrefs.Save();

      
        UpdateLanguageButton(localeCode);
    }

    private void UpdateLanguageButton(string localeCode)
    {
        languageButton.image.sprite = localeCode == "en" ? englishFlag : vietnameseFlag;
    }

    public static string GetCurrentLanguage()
    {
        
        return PlayerPrefs.GetString("Language", "en");
    }
}
