using UnityEngine;

[CreateAssetMenu(fileName = "LanguageData", menuName = "ScriptableObjects/New LanguageData", order = 1)]
public class LanguageData : ScriptableObject
{
    static string[] languages = new string[] { "English" };
    public static string[] Languages => languages;
    public static string CurrentLanguage => PlayerPrefs.GetString("Language", "Spanish");
    public static void SetLanguageStatic(string language) {
        PlayerPrefs.SetString("Language", language);
        PlayerPrefs.Save();
    }
    public void SetLanguage(string language) => SetLanguageStatic(language);
}
