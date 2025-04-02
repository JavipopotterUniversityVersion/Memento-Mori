using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TextPositionFixer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] Transform _origin;

    [SerializeField] Vector2 margin = new Vector2(85, 230);

    [SerializeField] Camera cam;
    // [SerializeField] LineRenderer _lineRenderer;

    private void Start() {
        // _lineRenderer.positionCount = 2;
    }

    void Update()
    {
        // _lineRenderer.SetPosition(0, _origin.position);

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
        Vector3 destination = cam.ScreenToWorldPoint(new Vector3(newPos.x, newPos.y - _text.rectTransform.rect.height/2, 0));
        // _lineRenderer.SetPosition(1, destination);
    }
}
