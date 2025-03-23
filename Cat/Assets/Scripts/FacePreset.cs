using UnityEngine;

[CreateAssetMenu(fileName = "FacePreset", menuName = "FacePreset", order = 0)]
public class FacePreset : ScriptableObject
{
    public Texture2D eyes;
    public Texture2D closed_eyes;
    public Texture2D mouth;
    public Texture2D opened_mouth;
}
