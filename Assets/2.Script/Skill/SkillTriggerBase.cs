using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class SkillTriggerBase : SkillBase, IActiveSkill,ISetInspector
{
    private List<ISkillTrigger> activeSkills = new ();
    protected PlayerBase player;
    
    [Button, GUIColor(0,1,0)]
    public void SetInspector()
    {
        activeSkills.Clear();
        var skillTrigger = GetComponent<ISkillTrigger>();
        if (null != skillTrigger)
            activeSkills.Add(skillTrigger);
        var triggers = GetComponentsInChildren<ISkillTrigger>();
        foreach (var t in triggers)
            activeSkills.Add(t);
    }
    public override void Spawned()
    {
        base.Spawned();
        player = App.I.GetPlayerInfo(Object.InputAuthority).PlayerObject.GetComponent<PlayerBase>();
        player.Inventory.AddSkill(this);
    }

    public void UpdateState()
    {
        foreach (var skillTrigger in activeSkills)
        {
            skillTrigger.UpdateState();
        }
    }


}