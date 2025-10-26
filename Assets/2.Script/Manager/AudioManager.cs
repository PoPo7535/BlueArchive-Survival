using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;
using Utility;

public class AudioManager : SerializeSingleton<AudioManager>
{
    [OdinSerialize] public Dictionary<string, int> test = new();

}
