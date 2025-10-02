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
    public SkillData SkillData => GameManager.I.skillDataTests[type].SkillData[_currentLevel - 1];
    public bool CanAttack => SkillData.delay <= delay;
    public bool CanLevelUp => _currentLevel < GameManager.I.skillDataTests[type].MaxLevel;

    protected PlayerBase player;
    [Serial] public SkillType type;
    [NonSerialized] public float delay = 0;
    private int _currentLevel = 1;

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
    public override void FixedUpdateNetwork()
    {
        if (false == CanAttack)
            delay += Runner.DeltaTime;
    }
    public void LevelUp()
    {
        if(CanLevelUp)
            ++_currentLevel;
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