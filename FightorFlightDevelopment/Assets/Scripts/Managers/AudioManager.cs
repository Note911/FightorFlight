using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager {

    private  Dictionary<string, AudioClip> _audioClips;

    public AudioManager() {
        _audioClips = new Dictionary<string, AudioClip>();
    }

    public void AddAudioClip(string name, AudioClip clip) {
        _audioClips.Add(name, clip);
    }

    public void AddAudioClips(string name, AudioClip[] clips) {
        for(int i = 0; i < clips.Length; ++i) {
            string _name = name;
            _name = name + "_" + i;
            _audioClips.Add(_name, clips[i]);
        }
    }

    public AudioClip GetAudioClip(string name) {
        return _audioClips[name];
    }
}