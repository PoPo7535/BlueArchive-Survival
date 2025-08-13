using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class PlayerSkillInventory : PlayerComponent
{
    [Read, Serial] private List<IActiveSkill> activeSkills = new();
    public override void Init(PlayerBase player)
    {
        base.Init(player);
        Player.inventory = this;
    }

    public void AddSkill(IActiveSkill skill)
    {
        skill.Init(this);
        activeSkills.Add(skill);
    }

    public void StateUpdate()
    {
        foreach (var skill in activeSkills)
        {
            skill.StatusUpdate();
        }
    }
}
