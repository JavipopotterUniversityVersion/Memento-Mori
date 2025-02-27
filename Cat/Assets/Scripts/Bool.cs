using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Bool", menuName = "Bool")]
public class Bool : ScriptableObject
{
    [SerializeField] bool _value;
    public bool Value
    {
        private set
        {
            if (_value == value) return;
            _value = value;
            _onValueChanged.Invoke(_value);
        }
        get => _value;
    }

    [SerializeField] UnityEvent<bool> _onValueChanged;

    public void Set(bool value) => Value = value;
    public void Toggle() => Value = !Value;

    public void Suscribe(UnityAction<bool> action) => _onValueChanged.AddListener(action);
    public void Unsuscribe(UnityAction<bool> action) => _onValueChanged.RemoveListener(action);
}
