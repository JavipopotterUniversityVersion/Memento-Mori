using UnityEngine;

public class ClausController : MonoBehaviour
{
    Animator animator;
    [SerializeField] SkinnedMeshRenderer _bodyRenderer;

    [SerializeField] Material[] _bodyMaterials;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void Set(int keyIndex) {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            keyIndex = 0;
            _bodyRenderer.material = _bodyMaterials[0];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            keyIndex = 1;
            _bodyRenderer.material = _bodyMaterials[1];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            keyIndex = 2;
            _bodyRenderer.material = _bodyMaterials[1];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            keyIndex = 3;
            _bodyRenderer.material = _bodyMaterials[0];
        }
        else if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            keyIndex = 4;
            _bodyRenderer.material = _bodyMaterials[1];
        }
        else if(Input.GetKeyDown(KeyCode.Alpha6))
        {
            keyIndex = 5;
            _bodyRenderer.material = _bodyMaterials[1];
        }
        
        if(keyIndex != -1) animator.SetTrigger(keyIndex.ToString());
    }
}
