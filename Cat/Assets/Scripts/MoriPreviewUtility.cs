using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MoriPreviewUtility : MonoBehaviour
{
    [SerializeField] Material _currentFace;
    [HideInInspector] FacePreset[] _facePresets;
    public void SetFacePresetsArray(FacePreset[] facePresets) => _facePresets = facePresets;
    public FacePreset[] GetFacePresetsArray() => _facePresets;
    public String[] dialogues;
    [SerializeField] DialogueInterpreter _interpreter;
    public DialogueInterpreter Interpreter => _interpreter;

    public void SetFacePreset(FacePreset facePreset)
    {
        if (facePreset == null) return;

        _currentFace.SetTexture("_Eyes", facePreset.eyes);
        _currentFace.SetTexture("_Mouth", facePreset.mouth);
        _currentFace.SetTexture("_Eyebrows", facePreset.eyebrows);
        _currentFace.SetTexture("_Pupils", facePreset.pupils);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(MoriPreviewUtility))]
public class MoriPreviewUtilityCustomInspector : Editor
{
    String _selectedDialogue;
    public override void OnInspectorGUI()
    {
        MoriPreviewUtility moriPreviewUtility = target as MoriPreviewUtility;
        moriPreviewUtility.SetFacePresetsArray(Resources.LoadAll<FacePreset>("FacePresets"));

        DrawDefaultInspector();

        GUILayout.Label("Faces", EditorStyles.boldLabel);
        foreach(var facePreset in moriPreviewUtility.GetFacePresetsArray())
        {
            if (GUILayout.Button(facePreset.name))
            {
                moriPreviewUtility.SetFacePreset(facePreset);
            }
        }

        GUILayout.Space(10);
        GUILayout.Label("Dialogues", EditorStyles.boldLabel);

        foreach(var dialogue in moriPreviewUtility.dialogues)
        {
            if (GUILayout.Button(dialogue.name))
            {
                Selection.activeObject = dialogue;
                _selectedDialogue = dialogue;
            }
        }

        GUILayout.Space(10);
        GUILayout.Label("Animation Names", EditorStyles.boldLabel);

        // var style = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter};

        ClausController _controller = moriPreviewUtility.GetComponent<ClausController>();
        foreach(var pair in _controller.TriggerNames)
        {
            GUILayout.Label(pair.Key + " : " + pair.Value);
        }

        GUILayout.Space(10);
        GUILayout.Label("Play Dialogue", EditorStyles.boldLabel);

        if(moriPreviewUtility.Interpreter == null)
        {
            GUILayout.Label("No interpreter found on this GameObject.");
            return;
        }

        if(_selectedDialogue == null)
        {
            GUILayout.Label("Select a dialogue to play it.");
            return;
        }

        if(GUILayout.Button("Play: " + _selectedDialogue.name))
        {
            moriPreviewUtility.Interpreter.StopAllCoroutines();
            moriPreviewUtility.Interpreter.Continue();
            moriPreviewUtility.Interpreter.StartDialogue(_selectedDialogue);
        }

        GUILayout.Space(15);
        GUILayout.Label("Controls", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        if(moriPreviewUtility.Interpreter.IsPaused())
        {
            if(GUILayout.Button("Resume")) moriPreviewUtility.Interpreter.Continue();
        }
        else
        {
            if(GUILayout.Button("Pause"))moriPreviewUtility.Interpreter.Pause();
        }

        if(moriPreviewUtility.Interpreter.IsSkip())
        {
            if(GUILayout.Button("Stop Skip")) moriPreviewUtility.Interpreter.SetSkip(false);
        }
        else
        {
            if(GUILayout.Button("Skip"))moriPreviewUtility.Interpreter.SetSkip(true);
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(20);
        GUILayout.Label("Dialogue Lines", EditorStyles.boldLabel);

        string[] selectedDialogueLines = _selectedDialogue.Value.Split("---", StringSplitOptions.RemoveEmptyEntries);

        foreach(var line in selectedDialogueLines)
        {
            if(RemoveCommands(line, out string output) == false) continue;
            if(GUILayout.Button(output))
            {
                moriPreviewUtility.Interpreter.StopAllCoroutines();
                moriPreviewUtility.Interpreter.StartDialogue(_selectedDialogue);
                moriPreviewUtility.Interpreter.SetLineIndex(Array.IndexOf(selectedDialogueLines, line));
                moriPreviewUtility.Interpreter.Continue();
            }
        }
    }

    bool RemoveCommands(string input, out string output)
    {
        output = input;
        if (input.Contains("<") && input.Contains(">"))
        {
            int startIndex = input.IndexOf("<");
            int endIndex = input.IndexOf(">");
            output = input.Remove(startIndex, endIndex - startIndex + 1);
            RemoveCommands(output, out output);
        }
        if(string.IsNullOrWhiteSpace(output)) return false;
        return true;
    }
}
#endif