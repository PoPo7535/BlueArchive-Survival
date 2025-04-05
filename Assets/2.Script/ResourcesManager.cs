using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class ResourcesManager : SerializedMonoBehaviour, ISetInspector
{
    [Read, OdinSerialize] private Dictionary<PlayableChar, TTTT> dic = new();
    
    [Button ,GUIColor(0,1,0)]
    public void SetInspector()
    {
        dic = new Dictionary<PlayableChar, TTTT>();
        var none = PlayableChar.None;
        foreach (var playable in none.ToArray().Skip(1))
        {
            dic.Add(playable, new TTTT
            {
                portrait = LoadHelper.LoadPortrait<Sprite>($"{playable.ToString()}"),
            });
        }

    }

    [Serializable]
    private class TTTT
    {
        public Sprite portrait;
        public Sprite portrait2;
    }
}
