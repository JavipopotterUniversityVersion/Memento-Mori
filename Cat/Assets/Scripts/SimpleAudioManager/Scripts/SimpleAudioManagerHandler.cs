using System;
using System.Collections;
using System.Collections.Generic;
using SimpleAudioManager;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Handler", menuName = "SimpleAudioManager/Handler", order = 0)]
public class SimpleAudioManagerHandler : ScriptableObject
{
    private UnityEvent<Song> _onPlaySong = new UnityEvent<Song>();
    public UnityEvent<Song> OnPlaySong =>_onPlaySong;

    private UnityEvent<float> _onStop = new UnityEvent<float>();
    public UnityEvent<float> OnStop => _onStop;

    private UnityEvent<bool> _onPause = new UnityEvent<bool>();
    public UnityEvent<bool> OnPause => _onPause;

    public void PlaySong(Song psong) => _onPlaySong.Invoke(psong);
    public void Stop(float fadeOutDuration) => _onStop.Invoke(fadeOutDuration);

    public void Pause(bool pause) => _onPause.Invoke(pause);

    public UnityEvent<float> _onSetPitch = new UnityEvent<float>();
    public UnityEvent<float> OnSetPitch => _onSetPitch;
}
