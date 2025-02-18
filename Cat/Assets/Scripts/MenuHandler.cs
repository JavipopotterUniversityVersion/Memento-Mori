using UnityEngine;
using UnityEngine.Events;

public class MenuHandler : MonoBehaviour
{
    bool paused = false;
    [SerializeField] UnityEvent<bool> _onPause;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pause(!paused);
        }
    }

    private void OnDestroy() {
        Time.timeScale = 1;
    }

    public void Pause(bool pause)
    {
        paused = pause;
        Time.timeScale = paused ? 0 : 1;
        _onPause.Invoke(pause);

        if(pause) Cursor.lockState = CursorLockMode.None;
        else Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = pause;
    }
}
