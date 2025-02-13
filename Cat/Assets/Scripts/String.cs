using UnityEngine;

[CreateAssetMenu(fileName = "String", menuName = "ScriptableObjects/New String", order = 1)]
public class String : ScriptableObject
{
    [SerializeField] [TextArea(30, 90)] string _value;
    public string Value => _value;
}
