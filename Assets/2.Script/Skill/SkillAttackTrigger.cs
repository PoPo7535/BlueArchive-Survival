using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class SkillAttackTrigger : MonoBehaviour
{
    private PlayerRef _otherPlayer;
    private AttackType _attackType = AttackType.Enter;

    public void Init()
    {
        
    }
    public void UpdateState()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(_attackType!= AttackType.Enter)
            return;
        if (false == other.CompareTag($"Monster"))
            return;

    }

    private void OnTriggerExit(Collider other)
    {
        if(_attackType!= AttackType.Enter)
            return;
        if (false == other.CompareTag($"Monster"))
            return;
    }

    private void OnTriggerStay(Collider other)
    {
        if(_attackType!= AttackType.Stay)
            return;
        if (false == other.CompareTag($"Monster"))
            return;
    }
}

public enum AttackType
{
    Enter,
    Stay
}