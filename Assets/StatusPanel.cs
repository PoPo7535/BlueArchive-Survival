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
        string path = "Assets/0.UI/UpGradeIcon/";
        
        foreach (PlayFabEx.StatusType type in Enum.GetValues(typeof(PlayFabEx.StatusType)))
        {
            $"{path}{type}_Icon".Log();
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>($"{path}{type}_Icon.png");
            dic.Add(type, sprite);
        }
    }
}
