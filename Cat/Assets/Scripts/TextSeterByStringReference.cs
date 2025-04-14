using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;



#if UNITY_EDITOR
using UnityEditor;
#endif

public class TextSeterByStringReference : MonoBehaviour
{
    [SerializeField] string _value;
    public void SetDefaultValue(string value) => _value = value;
    public string GetDefaultValue() => _value;

    public string Value {
        get {
            if (_languageDictionary.ContainsKey(LanguageData.CurrentLanguage)) return _languageDictionary[LanguageData.CurrentLanguage];
            else return _value;
        }
    }

    [SerializeField] SerializableDictionary<string, string> _languageDictionary = new SerializableDictionary<string, string>() { { "English", "" }};

    void Start()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = Value;
    }
}