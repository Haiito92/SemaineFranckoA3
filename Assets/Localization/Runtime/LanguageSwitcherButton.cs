using Localization.Runtime;
using UnityEngine;

public class LanguageSwitcherButton : MonoBehaviour
{
    [SerializeField] LANGUAGES_STATE language;
    LanguageManager languageManager;

    void Start()
    {
        languageManager = FindFirstObjectByType<LanguageManager>();
    }

    public void SwitchLanguage()
    {
        languageManager.LANGUAGEProperty = language;
    }
}
