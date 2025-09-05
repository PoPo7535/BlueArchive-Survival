using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


[CreateAssetMenu(fileName = "Skill Data", menuName = "Scriptable Object/Skill Data", order = int.MaxValue)]
public class SkillDataTest : ScriptableObject
{
    public NetworkObject skillObj;
    [Serial] private SkillData _skillData;
    public SkillData SkillData => _skillData;

}

[Serializable]
public struct SkillData
{
    public int skillID;
    public int skillMaxLevel;
}