using UnityEngine;
using TMPro;

public class OptionObject : MonoBehaviour
{
    BoxCollider col;
    public string dialogueName;
    public TextMeshPro textMesh;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Color _hoverColor;
    [SerializeField] Color _normalColor;

    private void Awake() {
        col = GetComponent<BoxCollider>();
        meshRenderer.material = new Material(meshRenderer.material);
        OnExit();
    }

    public void SetOption(string text, string dialogueName)
    {
        this.dialogueName = dialogueName;
        textMesh.text = text;
    }

    public BoxCollider GetCollider() => col;
    public void OnHover() => meshRenderer.material.color = _hoverColor;
    public void OnExit() => meshRenderer.material.color = _normalColor;
}
