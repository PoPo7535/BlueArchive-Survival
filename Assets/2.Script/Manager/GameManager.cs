using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using PlayFab.ClientModels;
using Fusion;
using TMPro;
using UnityEngine.Serialization;
using Utility;
#if UNITY_EDITOR   
using UnityEditor;
#endif
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;
public class GameManager : SerializeSingleton<GameManager>, ISetInspector
{
    [OdinSerialize] public Dictionary<PlayableChar, GameObject> playableChar = new ();
    [Read, Serial] private Dictionary<SkillType, SkillScriptable> _skillScriptables = new();
    public NetworkObject playerBase;
    public PlayerInfo playerInfo;
    public HitUi textUi;
    [NonSerialized] public EntityTokenResponse playFabEntity;
    [Read] public string ID;
    
    [Button, GUIColor(0,1,0)]
    public void SetInspector()
    {
#if UNITY_EDITOR   
        playableChar.Clear();
        var playableChars = PlayableChar.None.ToArray();
        foreach (var playable in playableChars.Skip(1))
        {
            var root = $"Assets/5.Prefab/Playable Character/{playable.ToString()}.prefab";
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(root);
            playableChar.Add(playable, obj);
        }
        _skillScriptables.Clear();
        var skillTypes = SkillType.None.ToArray();
        foreach (var skillType in skillTypes.Skip(1))
        {
            var root = $"Assets/3.ScriptableObject/{skillType.ToString()}.asset";
            var obj = AssetDatabase.LoadAssetAtPath<SkillScriptable>(root);
            obj._skillType = SkillType.Wheel;
            obj.SetSkillType();
            _skillScriptables.Add(skillType, obj);
        }
        
        var prefabRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(gameObject);
        PrefabUtility.ApplyPrefabInstance(prefabRoot , InteractionMode.UserAction);
#endif
    }

    public SkillScriptable GetSkillScriptable(SkillType skillType)
    {
        return _skillScriptables.GetValueOrDefault(skillType);
    }
}

public enum PlayableChar
{
    None,
    Kayoko,
    Neru,
    Miyu,
    Hoshino,
    Aris,
}
