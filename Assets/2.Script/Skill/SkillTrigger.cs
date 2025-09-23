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
    [Serial, Read] private TriggerType _triggerType;
    [Serial, Read] private SkillType _skillType;
    private PlayerBase _player;
    private ActiveSkillBase _activeSkill;
    private float _delay = 0;

    private float Damage => _player.State.GetDamageValue(_activeSkill.SkillData.damage);
    private bool CanAttack => _activeSkill.SkillData.delay <= _delay;
    public void Init(PlayerBase player, ActiveSkillBase activeSkillBase,TriggerType triggerType, SkillType skillType)
    {
        _player = player;
        _triggerType = triggerType;
        _skillType = skillType;
        _activeSkill = activeSkillBase;
    }
    public override void FixedUpdateNetwork()
    {
        if (false == CanAttack)
            _delay += Runner.DeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(false == CheckTrigger(other, TriggerType.Enter))
            return;

        if(other.TryGetComponent<MonsterHitManager>(out var hitManager))
            hitManager.Damage(Object.InputAuthority, Damage);
    }

    private void OnTriggerExit(Collider other)
    {
        if(false == CheckTrigger(other, TriggerType.Enter))
            return;
    }

    private void OnTriggerStay(Collider other)
    {
        if(false == CheckTrigger(other, TriggerType.Stay))
            return;
        
        var hitManager = other.GetComponent<MonsterHitManager>();
        while (CanAttack)
        {
            _delay -= _activeSkill.SkillData.delay;
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