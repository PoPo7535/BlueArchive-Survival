using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class SkillTrigger : NetworkBehaviour
{
    [Read, Serial] private AttackType _attackType;
    private float _delay = 10f;
    private float _MaxDelay = 10f;
    private bool CanAttack => _MaxDelay <= _delay;
    private HashSet<MonsterHitManager> dic = new();

    public  void Init(AttackType attackType)
    {
        _attackType = attackType;
    }
    public  void UpdateState()
    {
        
    }

    public override void FixedUpdateNetwork()
    {
        if (false == CanAttack)
            _delay += Runner.DeltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(false == CheckTrigger(other, AttackType.Enter))
            return;

        var hitManager = other.GetComponent<MonsterHitManager>();
        if (false == dic.Contains(hitManager))
        {
            hitManager.Damage(Object.InputAuthority, 10f);
            dic.Add(hitManager);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(false == CheckTrigger(other, AttackType.Enter))
            return;

        var hitManager = GetComponent<MonsterHitManager>();
        hitManager.Damage(Object.InputAuthority, 10f);
        if (dic.Contains(hitManager))
            dic.Remove(hitManager);
    }

    private void OnTriggerStay(Collider other)
    {
        if(false == CheckTrigger(other, AttackType.Stay))
            return;
        
        var hitManager = other.GetComponent<MonsterHitManager>();
        while (CanAttack)
        {
            _delay -= _MaxDelay;
            hitManager.Damage(Object.InputAuthority, 10f);
        }
    }

    private bool CheckTrigger(Collider other, AttackType type)
    {
        if (false == Object.HasStateAuthority)
            return false;
        if(_attackType!= type)
            return false;
        if (false == other.CompareTag($"Monster"))
            return false;
        return true;
    }
}

public enum AttackType
{
    Enter,
    Stay
}