using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class ActiveSkillBase : NetworkBehaviour, ISetInspector
{
    [Read, Serial, SerializeReference] protected List<SkillTrigger> activeSkills = new();
    public SkillScriptable skillScriptable => GameManager.I.GetSkillScriptable(type);
    public SkillData SkillData => skillScriptable.SkillData[currentLevel - 1];
    public bool CanLevelUp => currentLevel < skillScriptable.MaxLevel;
    public bool CanAttack => SkillData.delay <= delay;
    [NonSerialized] public int currentLevel = 1;
    [NonSerialized] public float delay = 0;

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
        player.SkillManager.AddSkill(this, type); 
        player.SkillManager.SkillUpdate();
    }
    public override void FixedUpdateNetwork()
    {
        if (false == CanAttack)
            delay += Runner.DeltaTime;
    }
    public void LevelUp()
    {
        if (CanLevelUp)
        {
            ++currentLevel;
            player.SkillManager.SkillUpdate();
            if (false == CanLevelUp)
                player.SkillManager.RemoveProbability(type);
        }
        else
            $"{nameof(LevelUp)} 오류".ErrorLog();
    }

    protected void SetPosition()
    {
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public void DecreaseDelay()
    {
        delay -= SkillData.delay;
    }

}