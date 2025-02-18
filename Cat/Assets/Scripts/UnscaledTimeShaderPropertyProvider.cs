using System.Collections;
using UnityEngine;

public class UnscaledTimeShaderPropertyProvider : MonoBehaviour
{
    [SerializeField] Material material;
    void OnEnable()
    {
        StartCoroutine(UpdateShaderProperty());
    }

    void OnDisable()
    {
        StopCoroutine(UpdateShaderProperty());
    }

    IEnumerator UpdateShaderProperty()
    {
        while (true)
        {
            material.SetFloat("_UnscaledTime", Time.unscaledTime);
            yield return null;
        }
    }
}
