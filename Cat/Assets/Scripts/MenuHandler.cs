using UnityEngine;
using UnityEngine.Events;

public class MenuHandler : MonoBehaviour
{
    bool paused = false;
    [SerializeField] UnityEvent<bool> _onToggle;
    [SerializeField] UnityEvent _onResume;
    [SerializeField] UnityEvent _onIsPaused;

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
        _onToggle.Invoke(pause);

        if(pause) 
        {
            Cursor.lockState = CursorLockMode.None;
            _onIsPaused.Invoke();
        }
        else 
        {
            Cursor.lockState = CursorLockMode.Locked;
            _onResume.Invoke();
        }

        Cursor.visible = pause;
    }
}
