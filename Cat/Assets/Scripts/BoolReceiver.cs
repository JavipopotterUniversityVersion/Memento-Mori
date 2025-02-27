using UnityEngine;
using UnityEngine.Events;

public class BoolReceiver : MonoBehaviour
{
    [SerializeField] Bool _bool;
    [SerializeField] UnityEvent<bool> _onValueChanged;
    [SerializeField] UnityEvent _onTrue;
    [SerializeField] UnityEvent _onFalse;

    void OnEnable() => _bool.Suscribe(OnValueChanged);
    void OnDisable() => _bool.Unsuscribe(OnValueChanged);

    public void Call() => OnValueChanged(_bool.Value);

    void OnValueChanged(bool value)
    {
        _onValueChanged.Invoke(value);
        if (value) _onTrue.Invoke();
        else _onFalse.Invoke();
    }
}
