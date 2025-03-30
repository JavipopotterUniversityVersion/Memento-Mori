using System.Collections;
using UnityEngine;

public class ClausController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] SkinnedMeshRenderer _bodyRenderer;

    [SerializeField] Material _currentFace;
    [SerializeField] SerializableDictionary<string, int> _triggerNames;
    [SerializeField] Vector2[] _eyesPresets;

    [SerializeField] FacePreset _currentFacePreset;
    int _iterationsToChangeMouth = 1;
    bool _mouthOpened = false;

    private void Start() {
        StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine() {
        while(true) {
            yield return new WaitForSeconds(Random.Range(1.0f, 5.0f));
            int blinkTimes = Random.Range(1, 3);

            for(int i = 0; i < blinkTimes; i++) {
                _currentFace.SetTexture("_Eyes", _currentFacePreset.closed_eyes);
                yield return new WaitForSeconds(0.1f);
                _currentFace.SetTexture("_Eyes", _currentFacePreset.eyes);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public void SetCurrentFace(FacePreset facePreset) {
        // _currentFace.mainTexture = texture;
        _currentFacePreset = facePreset;
        _currentFace.SetTexture("_Eyes", facePreset.eyes);
        _currentFace.SetTexture("_Mouth", facePreset.mouth);
        _currentFace.SetTexture("_Eyebrows", facePreset.eyebrows);
        _currentFace.SetTexture("_Pupils", facePreset.pupils);
    }

    public void MoveEyes(int index) {
        if(index < _eyesPresets.Length) {
            MoveEyes(_eyesPresets[index].x, _eyesPresets[index].y);
        }
    }

    public void Talk() {
        _iterationsToChangeMouth--;
        if(_iterationsToChangeMouth >= 0) return;
        _iterationsToChangeMouth = 1;

        if(_mouthOpened) _currentFace.SetTexture("_Mouth", _currentFacePreset.mouth);
        else _currentFace.SetTexture("_Mouth", _currentFacePreset.opened_mouth);

        _mouthOpened = !_mouthOpened;
    }

    public void StopTalking() {
        _currentFace.SetTexture("_Mouth", _currentFacePreset.mouth);
        _mouthOpened = false;
    }

    void MoveEyes(float deltar, float deltal) {
        StartCoroutine(MoveEyesRoutine(deltar, deltal));
    }
    IEnumerator MoveEyesRoutine(float deltar, float deltal) {
        float time = 0;
        float duration = 0.5f;
        float startR = _currentFace.GetFloat("_R_Rot");
        float startL = _currentFace.GetFloat("_L_Rot");

        while(time < duration) {
            time += Time.deltaTime;
            _currentFace.SetFloat("_R_Rot", Mathf.Lerp(startR, deltar, time / duration));
            _currentFace.SetFloat("_L_Rot", Mathf.Lerp(startL, deltal, time / duration));
            yield return new WaitForEndOfFrame();
        }
    }

    public void Set(string triggerName) {
        if(_triggerNames.ContainsKey(triggerName)) animator.SetTrigger(_triggerNames[triggerName].ToString());
    }

    public void Set(int keyIndex) {
        if(keyIndex != -1) animator.SetTrigger(keyIndex.ToString());
    }
}
