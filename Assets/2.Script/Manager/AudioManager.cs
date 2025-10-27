using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Utility;

public class AudioManager : SerializeSingleton<AudioManager>, ISetInspector
{
    [OdinSerialize] public Dictionary<string, AudioSource> audioBank = new();
    [Button, GUIColor(0,1,0)]
    public void SetInspector() { }

    public void PlayAudio(string audioName)
    {
        audioBank.TryGetValue(audioName, out var audio);
        {
            
        }
    }

    private void Start()
    {
        throw new NotImplementedException();
    }

    enum AudioType
    {
        BGM,
        SFX,
        Voice
    }
}
