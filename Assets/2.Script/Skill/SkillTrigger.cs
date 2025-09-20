using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class SkillTrigger : NetworkBehaviour
{
    [Serial] private TriggerType _triggerType;
    [Serial] private SkillType _skillType;
    private HashSet<MonsterHitManager> dic = new();
    private PlayerBase _player;
    private SkillData SkillData => GameManager.I.skillDataTests[_skillType].SkillData[currentLevel];
    private int currentLevel = 0;
    private float _delay = 0;
    private float Damage => _player.State.GetDamageValue(SkillData.damage);
    private bool CanAttack => SkillData.delay <= _delay;
    public bool CanLevelUp => currentLevel <= GameManager.I.skillDataTests[_skillType].maxLevel;
    public void Init(PlayerBase player, TriggerType triggerType, SkillType skillType)
    {
        _player = player;
        _triggerType = triggerType;
        _skillType = skillType;
    }
    public override void FixedUpdateNetwork()
    {
        if (false == CanAttack)
            _delay += Runner.DeltaTime;
    }

    public void LevelUp()
    {
        if (CanLevelUp)
            ++currentLevel;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(false == CheckTrigger(other, TriggerType.Enter))
            return;

        if(other.TryGetComponent<MonsterHitManager>(out var hitManager))
            hitManager.Damage(Object.InputAuthority, Damage);
        // if (false == dic.Contains(hitManager))
        // {
        //     dic.Add(hitManager);
        // }
    }

    private void OnTriggerExit(Collider other)
    {
        if(false == CheckTrigger(other, TriggerType.Enter))
            return;

        // var hitManager = GetComponent<MonsterHitManager>();
        // if (dic.Contains(hitManager))
        //     dic.Remove(hitManager);
    }

    private void OnTriggerStay(Collider other)
    {
        if(false == CheckTrigger(other, TriggerType.Stay))
            return;
        
        var hitManager = other.GetComponent<MonsterHitManager>();
        while (CanAttack)
        {
            _delay -= SkillData.delay;
            hitManager.Damage(Object.InputAuthority, 10f);
        }
    }

    private bool CheckTrigger(Collider other, TriggerType type)
    {
        if (false == Object.HasStateAuthority ||
            _triggerType != type ||
            false == other.CompareTag($"Monster"))
            return false;
        
        return true;
    }
}

public enum TriggerType
{
    Enter,
    Stay
}