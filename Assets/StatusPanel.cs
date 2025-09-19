using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class StatusPanel : SerializedMonoBehaviour, ISetInspector
{
    [Serial, Read] private Dictionary<PlayFabEx.StatusType, Sprite> dic;
    [Button, GUIColor(0,1,0)]
    public void SetInspector()
    {
        dic = new Dictionary<PlayFabEx.StatusType, Sprite>();
        foreach (PlayFabEx.StatusType type in Enum.GetValues(typeof(PlayFabEx.StatusType)))
        {
            var sprite = LoadHelper.LoadUpGradeIcon<Sprite>($"{type}_Icon");
            dic.Add(type, sprite);
        }
    }
}
