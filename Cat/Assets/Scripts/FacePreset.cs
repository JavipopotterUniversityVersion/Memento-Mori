using UnityEngine;

[CreateAssetMenu(fileName = "FacePreset", menuName = "FacePreset", order = 0)]
public class FacePreset : ScriptableObject
{
    public Texture2D eyebrows;
    public Texture2D eyes;
    public Texture2D closed_eyes;
    public Texture2D mouth;
    public Texture2D opened_mouth;
    public Texture2D pupils;

    [ContextMenu("SetFace")]
    public void SetFace()
    {
        Material face = Resources.Load<Material>("CurrentFace");
        if (face == null) Debug.LogError("Face material not found!");
        else
        {
            face.SetTexture("_Eyes", eyes);
            face.SetTexture("_Mouth", mouth);
            face.SetTexture("_Eyebrows", eyebrows);
            face.SetTexture("_Pupils", pupils);
        }
    }
}
