using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Audio;
using Utility;

public class AudioManager : SerializeSingleton<AudioManager>, ISetInspector
{
    [OdinSerialize] public Dictionary<string, AudioSource> audioBank = new();
    public AudioMixer mixer;
    [Button, GUIColor(0,1,0)]
    public void SetInspector() { }

    public void PlayAudio(Transform tr, string audioName)
    {
        if(audioBank.TryGetValue(audioName, out var audio))
        {
            
        }
    }

    enum AudioType
    {
        BGM,
        SFX,
        Voice
    }
    public AudioSource asd;
    [Button]
    public void Poo(float soundValue)
    {
        if (soundValue == 0)
        {
            mixer.SetFloat("BGM", -80f);
            return;
        }
        mixer.SetFloat("BGM", soundValue * 100 - 80f);
    }
}
