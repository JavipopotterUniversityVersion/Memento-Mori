using UnityEngine;
using UnityEngine.Events;

public class EventOnStart : MonoBehaviour
{
    [SerializeField] UnityEvent _onStart;

    private void Start() {
        _onStart.Invoke();
    }
}
