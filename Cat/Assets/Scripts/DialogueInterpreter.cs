using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class DialogueInterpreter : MonoBehaviour
{
    [SerializeField] LineRenderer _lineRenderer;

    [SerializeField] TextMeshProUGUI[] _channels;
    int channelIndex = 0;
    TextMeshProUGUI _text => _channels[channelIndex];

    [SerializeField] float timeBetweenChars = 0.05f;
    [SerializeField] float timeBetweenLines = 1.0f;
    [SerializeField] UnityEvent _onCharWritten;
    [SerializeField] Volume volume;
    SceneLoader sceneLoader;
    [SerializeField] ClausController clausController;

    [SerializeField] SimpleAudioManagerHandler audioHandler;
    [SerializeField] SerializableDictionary<string, Material> materials;
    [SerializeField] SerializableDictionary<string, Color> colors;

    [SerializeField] SerializableDictionary<string, UnityEvent> events;
    UnityEvent<string> onSendOptions = new UnityEvent<string>();
    public UnityEvent<string> OnSendOptions => onSendOptions;
    string dialogueCache;
    [SerializeField] String[] dialogues;
    Dictionary<string, String> dialogueDictionary = new Dictionary<string, String>();

    Dictionary<string, bool> _localBools = new Dictionary<string, bool>();

    bool _stop;
    public void Continue() => _stop = false;
    public void Pause() => _stop = true;
    public bool IsPaused() => _stop;

    bool _skip = false;
    public void SetSkip(bool skip) => _skip = skip;
    public bool IsSkip() => _skip;

    int _lineIndex = 0;
    
    private void Awake() {
        sceneLoader = GetComponent<SceneLoader>();
        foreach(var dialogue in dialogues) dialogueDictionary.Add(dialogue.name, dialogue);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.R))
        {
            StopAllCoroutines();
            StartCoroutine(StartDialogueRoutine(dialogueCache));
        }
    }

    public void StartDialogue(String dialogue, int index) => StartCoroutine(StartDialogueRoutine(dialogue.Value, index));
    public void StartDialogue(StringContainer dialogue) => StartCoroutine(StartDialogueRoutine(dialogue.Value));
    public void StartDialogue(String dialogue) => StartCoroutine(StartDialogueRoutine(dialogue.Value));
    public void StartDialogue(string dialogue) => StartCoroutine(StartDialogueRoutine(dialogueDictionary[dialogue].Value));

    struct CommandData
    {
        public string command;
        public int index;
    }

    IEnumerator StartDialogueRoutine(string dialogue, int index = 0)
    {   
        _localBools.Clear();

        channelIndex = 0;
        dialogueCache = dialogue;
        _lineRenderer.enabled = true;
        string[] lines = dialogue.Split($"---");

        for(_lineIndex = index; _lineIndex < lines.Length; _lineIndex++)
        {
            string line = lines[_lineIndex];
            string preProcessedLine = line.Trim();
            _text.maxVisibleCharacters = 0;
            float waitTime = timeBetweenLines;
            _text.text = CleanTags(preProcessedLine);

            for(int i = 0; i < preProcessedLine.Length; i++)
            {
                _text.maxVisibleCharacters++;

                if(preProcessedLine[i] == '<'){
                    string value = preProcessedLine.Split("<")[1].Split(">")[0];
                    preProcessedLine = preProcessedLine.Replace("<" + value + ">", "");
                    i--;

                    ReadCommand(value, ref waitTime);
                }
                else if(_text.text[i] != ' ' && !Input.GetKey(KeyCode.Tab) && channelIndex == 0){
                    _onCharWritten.Invoke();
                    clausController.Talk();
                }

                if(Input.GetKey(KeyCode.Tab) || _skip) yield return null;
                else yield return new WaitForSeconds(timeBetweenChars);
            }
            clausController.StopTalking();
            yield return new WaitWhile(() => _stop);
            if(Input.GetKey(KeyCode.Tab) || _skip) yield return null;
            else yield return new WaitForSeconds(waitTime);
        }

         _text.maxVisibleCharacters = 0;
        _lineRenderer.enabled = false;
    }

    string CleanTags(string text)
    {
        string[] tags = text.Split("<");
        string result = "";
        for(int i = 0; i < tags.Length; i++)
        {
            if(tags[i].Contains(">"))
            {
                string value = tags[i].Split(">")[1];
                result += value;
            }
            else result += tags[i];
        }
        return result;
    }

    public void ReadCommand(string command)
    {
        float refTime = timeBetweenLines;
        ReadCommand(command, ref refTime);
    }

    public void ReadCommand(string command, ref float waitTime)
    {
        string value = command.Split(":", 2)[0].Trim();
        string arg = command.Split(":", 2)[1].Trim();

        if(value == "c")
        {
            if(int.TryParse(arg, out int result)) clausController.Set(result);
            else clausController.Set(arg);
        }
        else if(value == "f")
        {
            // clausController.SetCurrentFace(Resources.Load<Texture2D>("Textures/" + arg));
            clausController.SetCurrentFace(Resources.Load<FacePreset>("FacePresets/" + arg));
        }
        else if(value == "e")
        {
            clausController.MoveEyes(int.Parse(arg));
        }
        else if(value == "pitch") audioHandler.OnSetPitch.Invoke(float.Parse(arg));
        else if(value == "event") {
            if(events.ContainsKey(arg)) events[arg].Invoke();
        }
        else if(value == "w") waitTime = float.Parse(arg);
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
            string[] subArgs = arg.Split("/");
            if(subArgs.Length > 1)
            {
                if(subArgs.Length > 2)sceneLoader.LoadScene(subArgs[0], subArgs[1] == "white", () => ReadCommand(subArgs[2]));
                else sceneLoader.LoadScene(subArgs[0], subArgs[1] == "white");
            }
            else sceneLoader.LoadScene(arg);
        }
        else if(value == "iload") sceneLoader.LoadSceneInstantly(arg);
        else if(value == "set")
        {
            string stringRef = arg.Split("=")[0];
            string dialogueName = arg.Split("=")[1];
            StringContainer container = Resources.Load<StringContainer>("Dialogues/" + stringRef);
            print(stringRef);
            print(container);
            container.SetValue(dialogueDictionary[dialogueName]);
        }
        else if(value == "play")
        {
            AudioPlayer audio = Resources.Load<AudioPlayer>("SoundPlayers/" + arg);
            
            bool oneShot = true;
            if(arg.Split("/").Length > 1) oneShot = int.Parse(arg.Split("/")[1]) == 1;

            if(oneShot) audio.Play();
            else audio.PlayMusic();
        }
        else if(value == "stop")
        {
            AudioPlayer audio = Resources.Load<AudioPlayer>("SoundPlayers/" + arg);
            audio.Stop();
        }
        else if(value == "stopOnLoad")
        {
            sceneLoader.StopAudioOnLoad(arg == "true");
        }
        else if(value == "charTime") timeBetweenChars = float.Parse(arg);
        else if(value == "setMat")
        {
            string[] subArgs = arg.Split("/");
            materials[subArgs[0]].SetFloat(subArgs[1], float.Parse(subArgs[2]));
        }
        else if(value == "setMatColor")
        {
            string[] subArgs = arg.Split("/");
            materials[subArgs[0]].SetColor(subArgs[1], colors[subArgs[2]]);
        }
        else if(value == "setMatBool")
        {
            string[] subArgs = arg.Split("/");
            materials[subArgs[0]].SetFloat(subArgs[1], subArgs[2] == "true" ? 1 : 0);
        }
        else if(value == "Bool")
        {
            string[] subArgs = arg.Split("/");
            Bool boolRef = Resources.Load<Bool>(subArgs[0]);
            boolRef.Set(subArgs[1] == "true");
        }
        else if(value == "bool")
        {
            string[] subArgs = arg.Split("/");
            if(_localBools.ContainsKey(subArgs[0])) _localBools[subArgs[0]] = subArgs[1] == "true";
            else _localBools.Add(subArgs[0], subArgs[1] == "true");
        }
    }
}
