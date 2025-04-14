using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;



#if UNITY_EDITOR
using UnityEditor;
#endif


[CreateAssetMenu(fileName = "String", menuName = "ScriptableObjects/New String", order = 1)]
public class String : ScriptableObject
{
    [SerializeField] [TextArea(30, 90)] string _value;
    public void SetDefaultValue(string value) => _value = value;
    public string GetDefaultValue() => _value;

    public string Value {
        get {
            if(LanguageData.CurrentLanguage == "Spanish") return GetDefaultValue();
            else return GetLanguageValue(LanguageData.CurrentLanguage);
        }
    }

    [SerializeField] Dictionary<string, string> _lDictionary = new Dictionary<string, string>() { { "English", "" }, { "Italian", "" }, { "French", ""} };
    [SerializeField] string[] _translationsArray = new string[10];
    public Dictionary<string, string> GetDictionary() => _lDictionary;
    public void SetLanguageValue(string language, string value)
    {
        if (_lDictionary.ContainsKey(language)) {
            int index = 0;
            
            foreach (var item in _lDictionary) {
                if (item.Key == language) {
                    _translationsArray[index] = value;
                    break;
                }
                index++;
            }

            _lDictionary[language] = value;
        }
    }

    public string GetLanguageValue(string language)
    {
        if (_lDictionary.ContainsKey(language)) {
            int index = 0;

            foreach (var item in _lDictionary) {
                if (item.Key == language) {
                    return _translationsArray[index];
                }
                index++;
            }

            return _translationsArray[index];
        }
        return string.Empty;
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(String))]
public class StringCustomInspector : Editor
{
    string _currentLanguage = "";
    string _currentText = string.Empty;

    public void OnEnable()
    {
        String stringObject = target as String;
        _currentLanguage = LanguageData.CurrentLanguage;
        _currentText = stringObject.GetLanguageValue(_currentLanguage);
    }

    private void OnDisable() {
        if (target == null) return;
        String stringObject = target as String;
        AssignCurrentTextToLanguage(stringObject);
    }

    void AssignCurrentTextToLanguage(String stringObject)
    {
        if(_currentLanguage == "Spanish") stringObject.SetDefaultValue(_currentText);
        else stringObject.SetLanguageValue(_currentLanguage, _currentText);

        EditorUtility.SetDirty(stringObject);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public override void OnInspectorGUI()
    {
        String stringObject = target as String;

        if(LanguageData.Languages.Length != stringObject.GetDictionary().Count)
        {
            foreach (var language in LanguageData.Languages)
            {
                if (!stringObject.GetDictionary().ContainsKey(language))
                {
                    stringObject.SetLanguageValue(language, string.Empty);
                }
            }
        }

        if (stringObject.GetDictionary().Count != LanguageData.Languages.Length)
        {
            foreach (var language in stringObject.GetDictionary().Keys.ToList())
            {
                if (!LanguageData.Languages.Contains(language))
                {
                    stringObject.GetDictionary().Remove(language);
                }
            }
        }
        // DrawDefaultInspector();

        EditorGUI.BeginChangeCheck();
        var textArea = GUILayout.TextArea(_currentText, GUILayout.Height(500));


        if (EditorGUI.EndChangeCheck())
        {
            _currentText = textArea;
            AssignCurrentTextToLanguage(stringObject);
        }

        GUILayout.Label("Current Language: " + _currentLanguage);

        if(GUILayout.Button("Spanish"))
        {
            GUILayout.Label("Language: Spanish");
            _currentText = stringObject.GetDefaultValue();
            _currentLanguage = "Spanish";
        }

        foreach (var language in LanguageData.Languages)
        {
            if(GUILayout.Button(language))
            {
                GUILayout.Label("Language: " + language, EditorStyles.boldLabel);
                _currentText = stringObject.GetLanguageValue(language);
                _currentLanguage = language;
            }
        }
    }
}
#endif
