using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public struct StringEventPair
{
    public string key;
    public UnityEvent value;
}

public class DialogueInterpreter : MonoBehaviour
{
    [SerializeField] LineRenderer _lineRenderer;

    [SerializeField] TextMeshProUGUI[] _channels;
    int channelIndex = 0;
    TextMeshProUGUI _text => _channels[channelIndex];

    [SerializeField] float timeBetweenChars = 0.05f;
    [SerializeField] float timeBetweenLines = 1.0f;
    [SerializeField] UnityEvent _onCharWritten;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Sprite[] sprites;
    [SerializeField] AudioSource musicSource;
    SceneLoader sceneLoader;

    [SerializeField] StringEventPair[] stringEventDictionary;
    Dictionary<string, UnityEvent> events = new Dictionary<string, UnityEvent>();
    UnityEvent<string> onSendOptions = new UnityEvent<string>();
    public UnityEvent<string> OnSendOptions => onSendOptions;
    bool _stop;

    string dialogueCache;

    public void Continue() => _stop = false;
    
    private void Awake() {
        sceneLoader = GetComponent<SceneLoader>();
        foreach(var value in stringEventDictionary)
        {
            events.Add(value.key, value.value);
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.R))
        {
            StopAllCoroutines();
            StartCoroutine(StartDialogueRoutine(dialogueCache));
        }
    }

    public void StartDialogue(StringContainer dialogue) => StartCoroutine(StartDialogueRoutine(dialogue.Value));
    public void StartDialogue(String dialogue) => StartCoroutine(StartDialogueRoutine(dialogue.Value));
    public void StartDialogue(string dialogue) => StartCoroutine(StartDialogueRoutine(Resources.Load<String>(dialogue).Value));
    IEnumerator StartDialogueRoutine(string dialogue)
    {
        channelIndex = 0;
        dialogueCache = dialogue;
        _lineRenderer.enabled = true;
        string[] lines = dialogue.Split($"---");

        foreach(string line in lines)
        {
            _text.text = line;
            _text.maxVisibleCharacters = 0;
            float waitTime = timeBetweenLines;

            for(int i = 0; i < _text.text.Length; i++)
            {
                _text.maxVisibleCharacters++;

                if(_text.text[i] == '<')
                {
                    string value = _text.text.Split("<")[1].Split(">")[0];
                    _text.text = _text.text.Replace("<" + value + ">", "");
                    ReadCommand(value, ref waitTime);
                } else if(_text.text[i] != ' ') _onCharWritten.Invoke();

                yield return new WaitForSeconds(timeBetweenChars);
            }
            yield return new WaitWhile(() => _stop);
            yield return new WaitForSeconds(waitTime);
        }

         _text.maxVisibleCharacters = 0;
        _lineRenderer.enabled = false;
    }

    public void ReadCommand(string command, ref float waitTime)
    {
        string value = command.Split(":")[0];
        string arg = command.Split(":")[1];

        if(value == "c")
        {
            sr.sprite = sprites[int.Parse(arg)];
        }
        else if(value == "pitch")
        {
            musicSource.pitch = float.Parse(arg);
        }
        else if(value == "event")
        {
            events[arg].Invoke();
        }
        else if(value == "w")
        {
            waitTime = float.Parse(arg);
        }
        else if(value == "dialogue")
        {
            StopAllCoroutines();
            StartDialogue(Resources.Load<String>(arg));
        }
        else if(value == "opt")
        {
            onSendOptions.Invoke(arg);
            _stop = true;
        }
        else if(value == "channel")
        {
            _channels[channelIndex].text = "";
            channelIndex = int.Parse(arg);
        }
        else if(value == "load")
        {
            sceneLoader.LoadScene(arg);
        }
        else if(value == "set")
        {
            string stringRef = arg.Split(":=")[0];
            string newString = arg.Split(":=")[1];
            Resources.Load<StringContainer>(stringRef).SetValue(Resources.Load<String>(newString));
        }
    }
}
