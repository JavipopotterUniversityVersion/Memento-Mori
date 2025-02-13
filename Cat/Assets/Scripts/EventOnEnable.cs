using UnityEngine;
using UnityEngine.Events;

public class EventOnEnable : MonoBehaviour
{
    [SerializeField] UnityEvent _onEnable;
    private void OnEnable() {
        _onEnable.Invoke();
    }
}
