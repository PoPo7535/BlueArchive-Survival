using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Sirenix.OdinInspector;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class ActiveSkillGun : ActiveSkillBase, ISetInspector
{
    [Serial] private NetworkObject bullet;
    public new void SetInspector()
    {
        base.SetInspector();
    }
    public override void Spawned()
    {
        base.Spawned();
        transform.SetParent(player.transform, false);
        SetPosition();
    }
    
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        if(false == Object.HasStateAuthority) 
            return;

        if (CanAttack)
        {
            var targetObj = player.findEnemy.nearObj;
            if (null == targetObj)
                return;
            DecreaseDelay();

            var dir = (targetObj.transform.position - player.transform.position).normalized * 0.1f;
            var lookRotation = Quaternion.LookRotation(dir, Vector3.zero);
            Runner.Spawn(bullet, player.transform.position + dir, lookRotation,
                inputAuthority: Object.InputAuthority);
        }
    }
}
