using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;
#if UNITY_EDITOR
using UnityEditor;
#endif


[CreateAssetMenu(fileName = "Skill Data", menuName = "Scriptable Object/Skill Data", order = int.MaxValue)]
public class SkillScriptable : ScriptableObject
{
    public NetworkObject skillObj;
    [Serial] private SkillData _skillData;
    [Serial] public SkillType _skillType;
    public SkillData SkillData => _skillData;

    [Button, GUIColor(0, 1, 0)] 
    public void SetSkillType()
    {
#if UNITY_EDITOR
        var skillBase = skillObj.GetComponent<ActiveSkillBase>();
        skillBase.type = _skillType;
#endif
    }
}

[Serializable]
public struct SkillData
{
    public int skillMaxLevel;
}

public enum SkillType
{
    None,
    Wheel
}