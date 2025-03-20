using UnityEngine;

public class MirandaController : MonoBehaviour
{
    Animator animator;
    int index = 0;
    string[] animations = new string[] { "MONO", "LOOK_SIDES", "SCARED", "UWU"};

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.Play(animations[index]);
            index++;
            if (index >= animations.Length) index = 0;
        }
    }
}
