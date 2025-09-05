using System.Collections;
using System.Collections.Generic;
using Fusion;
using Sirenix.Serialization;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class PlayerSkillInventory : PlayerComponent
{
    [Read] private Dictionary<SkillType, IActiveSkill> activeSkills = new();
    // [Read] private List<IPassiveSkill> passiveSkills = new();
    public override void Init(PlayerBase player)
    {
        base.Init(player);
        Player.Inventory = this;
    }

    public void AddSkill(SkillType type, IActiveSkill skillTrigger)
    {
        activeSkills.Add(type, skillTrigger);
    }

    public void StateUpdate()
    {
        foreach (var skill in activeSkills.Values)
        {
            skill.UpdateState();
        }
    }

    public void GetSkill()
    {
        
    }
}
