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
    [FormerlySerializedAs("_attackType")] [Serial] private TriggerType triggerType;
    private HashSet<MonsterHitManager> dic = new();
    private PlayerBase _player;
    private float _delay = 10f;
    private float _MaxDelay = 10f;
    private float _damageValue = 100f;
    private float Damage => _player.State.GetDamageValue(_damageValue);
    private bool CanAttack => _MaxDelay <= _delay;

    public  void Init(PlayerBase _player, TriggerType triggerType)
    {
        this._player = _player;
        this.triggerType = triggerType;
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
            _delay -= _MaxDelay;
            hitManager.Damage(Object.InputAuthority, 10f);
        }
    }

    private bool CheckTrigger(Collider other, TriggerType type)
    {
        if (false == Object.HasStateAuthority ||
            triggerType != type ||
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