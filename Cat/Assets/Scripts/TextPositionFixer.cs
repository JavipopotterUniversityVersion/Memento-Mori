using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TextPositionFixer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] Transform _origin;

    [SerializeField] Vector2 margin = new Vector2(85, 230);

    [SerializeField] Camera cam;

    void Update()
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector2 screenPos = cam.WorldToViewportPoint(_origin.position);
        screenPos -= new Vector2(0.5f, 0.5f);

        Vector2 newPos = screenPos * screenSize;

        if(screenPos.x < -margin.x || screenPos.x > margin.x)
        {
            newPos.Set(_text.rectTransform.localPosition.x, newPos.y);
        }

        if(screenPos.y < -margin.y || screenPos.y > margin.y)
        {
            newPos.Set(newPos.x, _text.rectTransform.localPosition.y);
        }

        _text.rectTransform.localPosition = newPos;
    }
}
