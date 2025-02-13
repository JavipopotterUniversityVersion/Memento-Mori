using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] Image _fadeImage;
    public void LoadScene(string scene) => StartCoroutine(LoadSceneRoutine(scene));
    public void LoadSceneInstantly(string scene) => SceneManager.LoadScene(scene);

    IEnumerator LoadSceneRoutine(string scene)
    {
        for(float i = 0; i < 1; i += Time.deltaTime)
        {
            _fadeImage.color = new Color(0, 0, 0, i);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(scene);
    }

    public void Unfade() => StartCoroutine(UnfadeRoutine());
    IEnumerator UnfadeRoutine()
    {
        for(float i = 1; i > 0; i -= Time.deltaTime)
        {
            _fadeImage.color = new Color(0, 0, 0, i);
            yield return new WaitForEndOfFrame();
        }
    }

    public void Fade() => StartCoroutine(FadeRoutine());
    IEnumerator FadeRoutine()
    {
        for(float i = 0; i < 1; i += Time.deltaTime)
        {
            _fadeImage.color = new Color(0, 0, 0, i);
            yield return new WaitForEndOfFrame();
        }
    }
}
