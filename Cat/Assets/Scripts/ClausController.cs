using UnityEngine;

public class ClausController : MonoBehaviour
{
    Animator animator;
    [SerializeField] SkinnedMeshRenderer _bodyRenderer;

    [SerializeField] Material[] _bodyMaterials;
    [SerializeField] SerializableDictionary<string, int> _triggerNames;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void Set(string triggerName) {
        if(_triggerNames.ContainsKey(triggerName)) animator.SetTrigger(_triggerNames[triggerName].ToString());
    }

    public void Set(int keyIndex) {
        if(keyIndex != -1) animator.SetTrigger(keyIndex.ToString());
    }
}
