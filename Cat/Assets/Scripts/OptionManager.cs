using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    OptionObject[] options;
    [SerializeField] DialogueInterpreter interpreter;
    [SerializeField] Transform center;

    [SerializeField] float optionsDistance = 3.0f;
    [SerializeField] float optionsSpeed = 1.0f;
    [SerializeField] LayerMask targetLayer;

    OptionObject lastOption;


    private void Awake(){
        options = GetComponentsInChildren<OptionObject>();
        interpreter.OnSendOptions.AddListener(SetOptions);

        foreach(var option in options)
        {
            option.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100);
    }

    private void Update() {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, targetLayer);

        if(hit.collider && hit.transform.parent.TryGetComponent(out OptionObject opt))
        {
            if(lastOption != null && lastOption != opt)
            {
                lastOption.OnExit();
            }

            opt.OnHover();
            lastOption = opt;

            if(Input.GetMouseButtonDown(0)) 
            {
                if(opt.dialogueName != "follow") interpreter.StartDialogue(opt.dialogueName);

                interpreter.Continue();
                foreach(var option in options) option.gameObject.SetActive(false);
            }
        }
        else if(lastOption != null)
        {
            lastOption.OnExit();
            lastOption = null;
        }
    }
    void SetOptions(string inputOptions)
    {
        string[] opts = inputOptions.Split(",");

        float angle = 2 * MathF.PI / opts.Length;

        for(int i = 0; i < opts.Length; i++)
        {
            options[i].SetOption(opts[i].Split("/")[0], opts[i].Split("/", 2)[1]);
            Vector3 targetPos = ((transform.forward * math.cos((angle * i) + MathF.PI) + math.sin((angle * i) + MathF.PI) * transform.up) * optionsDistance) + center.localPosition;
            StartCoroutine(SetOptionAnimation(options[i].transform, targetPos));
        }
    }

    IEnumerator SetOptionAnimation(Transform option, Vector3 targetPos)
    {
        option.gameObject.SetActive(true);
        option.transform.localPosition = center.localPosition;

        while(Vector3.Distance(option.transform.localPosition, targetPos) > 0.025f)
        {
            float speedFactor = Vector3.Distance(option.transform.localPosition, targetPos) / optionsDistance;
            option.transform.localPosition = Vector3.Lerp(option.transform.localPosition, targetPos, speedFactor * optionsSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
