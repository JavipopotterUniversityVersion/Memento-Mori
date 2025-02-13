using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SimpleLineRendererConector : MonoBehaviour
{
    [SerializeField] Transform _start;
    [SerializeField] Transform _end;
    LineRenderer _line;

    private void Awake() {
        _line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        _line.SetPositions(new Vector3[2] {_start.position, _end.position});
    }
}
