using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Audio;
using Utility;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AudioManager : SerializeSingleton<AudioManager>, ISetInspector
{
    public AudioMixer mixer;
    [OdinSerialize] public Dictionary<string, AudioBank> audioBank;

    [Button, GUIColor(0, 1, 0)]
    public void SetInspector()
    {
#if UNITY_EDITOR
        audioBank = new Dictionary<string, AudioBank>();
        var root = $"Assets/3.Audio/";
        string[] guids = AssetDatabase.FindAssets("t:AudioClip", new[] { root });
        var arr = guids
             .Select(AssetDatabase.GUIDToAssetPath)
             .Select(AssetDatabase.LoadAssetAtPath<AudioClip>)
             .ToArray();

        var audioTypes = AudioType.Voice.ToArray();
    
        foreach (var audioClip in arr)
        {
            var split = audioClip.name.Split('_');
            var audioTypeName = split[0];
            var audioClipName = split[1];
            foreach (var audioType in audioTypes)
            {
                if (audioTypeName == audioType.ToString())
                {
                    audioBank.Add(audioClipName, new AudioBank
                    {
                        type = audioType,
                        clip = audioClip
                    });
                    break;
                }

                if (audioType.IsLast())
                {
                    $"{typeof(AudioManager)} 오디오 뱅크 초기화 오류".ErrorLog();
                }
            }
        }
#endif
    }

    public void Start()
    {
        mixer.SetFloat("BGM", PlayerPrefs.GetFloat("BGM"));
        mixer.SetFloat("SFX", PlayerPrefs.GetFloat("SFX"));
        mixer.SetFloat("Voice", PlayerPrefs.GetFloat("Voice"));
        mixer.SetFloat("Master", PlayerPrefs.GetFloat("Master"));
    }

    public void SetSoundValue(string soundName, float soundValue)
    {
        soundValue = soundValue * 100 - 80f;
        mixer.SetFloat(soundName, soundValue);
        PlayerPrefs.SetFloat(soundName, soundValue);
    }
    
    public void PlayAudio(Transform tr, string audioName)
    {
        if(audioBank.TryGetValue(audioName, out var audio))
        {
            
        }
        else
        {
            
        }
    }

    public struct AudioBank
    {
        public AudioType type;
        public AudioClip clip;
    }
    public enum AudioType
    {
        BGM,
        SFX,
        Voice
    }
}
