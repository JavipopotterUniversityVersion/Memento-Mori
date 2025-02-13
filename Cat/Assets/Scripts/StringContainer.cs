using UnityEngine;

[CreateAssetMenu(fileName = "StringContainer", menuName = "StringContainer", order = 0)]
public class StringContainer : ScriptableObject {
    
    [SerializeField] String _value;
    public string Value => _value.Value;

    public void SetValue(String value) => _value = value;
}
