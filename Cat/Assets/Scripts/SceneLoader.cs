using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] Image _fadeImage;
    bool _stopAudioOnLoad = true;

    public void StopAudioOnLoad(bool stop) => _stopAudioOnLoad = stop;
    public void LoadScene(string scene) => StartCoroutine(LoadSceneRoutine(scene));
    public void LoadScene(string scene, bool white = false, UnityAction action = null)
    {
        if(white) _fadeImage.color = Color.white;
        else _fadeImage.color = Color.black;

        StartCoroutine(LoadSceneRoutine(scene, action));
    }
    public void LoadSceneInstantly(string scene) 
    {
        if(_stopAudioOnLoad) AudioPerformer.Instance.StopAll();
        SceneManager.LoadScene(scene);
    }

    IEnumerator LoadSceneRoutine(string scene, UnityAction action = null)
    {
        EventSystem.current.enabled = false;

        for(float i = 0; i < 1; i += 0.01f)
        {
            _fadeImage.color = new Color(_fadeImage.color.r, _fadeImage.color.g, _fadeImage.color.b, i);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        _fadeImage.color = new Color(_fadeImage.color.r, _fadeImage.color.g, _fadeImage.color.b, 1);
        if(_stopAudioOnLoad) AudioPerformer.Instance.StopAll();
        yield return new WaitForSecondsRealtime(1.0f);
        action?.Invoke();

        SceneManager.LoadScene(scene);
    }

    public void Unfade() => StartCoroutine(UnfadeRoutine());
    IEnumerator UnfadeRoutine()
    {
        for(float i = 1; i > 0; i -= Time.deltaTime)
        {
            _fadeImage.color = new Color(_fadeImage.color.r, _fadeImage.color.g, _fadeImage.color.b, i);
            yield return new WaitForEndOfFrame();
        }
    }

    public void Fade() => StartCoroutine(FadeRoutine());
    public void Silence() => AudioPerformer.Instance.StopAll();
    
    IEnumerator FadeRoutine()
    {
        for(float i = 0; i < 1; i += Time.deltaTime)
        {
            _fadeImage.color = new Color(_fadeImage.color.r, _fadeImage.color.g, _fadeImage.color.b, i);
            yield return new WaitForEndOfFrame();
        }
        _fadeImage.color = new Color(_fadeImage.color.r, _fadeImage.color.g, _fadeImage.color.b, 1);
    }
}
