using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Sirenix.OdinInspector;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class ActiveSkillBase : NetworkBehaviour, ISetInspector
{
    [Read, Serial, SerializeReference] protected List<SkillTrigger> activeSkills = new();
    protected PlayerBase player;
    public int currentLevel = 1;
    [Serial] public SkillType type;
    public SkillData SkillData => GameManager.I.skillDataTests[type].SkillData[currentLevel - 1];
    public bool CanLevelUp
    {
        get
        {
            $"{currentLevel} < {GameManager.I.skillDataTests[type].MaxLevel}".Log();
            return currentLevel < GameManager.I.skillDataTests[type].MaxLevel;
        }
    }

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
        player.SkillManager.AddSkill(this, type); 
    }

    public void LevelUp()
    {
        if(CanLevelUp)
            ++currentLevel;
    }
}