using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;

public class LanguageCanvasHandler : MonoBehaviour
{
    [SerializeField] UnityEvent onLanguageChanged = new UnityEvent();
    private void Start() {
        Button[] buttons = GetComponentsInChildren<Button>(true);
        buttons.ToList().ForEach(b => b.gameObject.SetActive(false));

        buttons[0].gameObject.SetActive(true);
        buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Spanish";
        buttons[0].onClick.AddListener(() =>
        {
            LanguageData.SetLanguageStatic("Spanish");
            onLanguageChanged.Invoke();
        });

        for(int i = 0; i < LanguageData.Languages.Length; i++)
        {
            buttons[i+1].gameObject.SetActive(true);
            buttons[i+1].GetComponentInChildren<TextMeshProUGUI>().text = LanguageData.Languages[i];
            string language = LanguageData.Languages[i];
            buttons[i+1].onClick.AddListener(() =>
            {
                LanguageData.SetLanguageStatic(language);
                onLanguageChanged.Invoke();
            });
        }
    }

    private void OnValidate() {
        Button[] buttons = GetComponentsInChildren<Button>(true);
        buttons.ToList().ForEach(b => b.gameObject.name = "---");

        buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Spanish";
        buttons[0].name = "Spanish";

        for(int i = 0; i < LanguageData.Languages.Length; i++)
        {
            buttons[i+1].GetComponentInChildren<TextMeshProUGUI>().text = LanguageData.Languages[i];
            buttons[i+1].name = LanguageData.Languages[i];
        }
    }
}
