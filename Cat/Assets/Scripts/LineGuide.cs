using TMPro;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineGuide : MonoBehaviour
{
    [SerializeField] Transform _start;
    [SerializeField] RectTransform _end;
    [SerializeField] Camera cam;
    [SerializeField] float zOffset;
    LineRenderer _line;

    private void Awake() {
        _line = GetComponent<LineRenderer>();
    }

    private void Update() {
        Vector2 screenSize = new Vector2(Screen.width, Screen.width);

        Vector3 pos = _end.localPosition/screenSize;
        pos.Set(pos.x,pos.y,zOffset);

        pos += new Vector3(0.5f, 0.5f, 0);
        pos = cam.ViewportToWorldPoint(pos);

        _line.SetPositions(new Vector3[2] {_start.transform.position, pos});
    }
}
