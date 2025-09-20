using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class ActiveSkillBase : SkillBase, ISetInspector
{
    [Read, Serial, SerializeReference] protected List<SkillTrigger> activeSkills = new();
    [Serial] public SkillType type;
    protected PlayerBase player;

    [Button, GUIColor(0,1,0)]
    public void SetInspector()
    {
        activeSkills.Clear();
        var triggers = GetComponentsInChildren<SkillTrigger>();
        foreach (var t in triggers)
            activeSkills.Add(t);
    }
    public override void Spawned()
    {
        base.Spawned();
        player = App.I.GetPlayerInfo(Object.InputAuthority).PlayerObject.GetComponent<PlayerBase>();
        player.Inventory.AddSkill(type, this); 
    }

}