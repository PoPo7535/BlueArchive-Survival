using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
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
        SetPosition();
    }
    public override void FixedUpdateNetwork()
    {
        if(false == HasStateAuthority) 
            return;
        if (CanAttack)
        {
            var obj = player.findEnemy.nearObj;
            if (null == obj)
                return;
            DecreaseDelay();

            Runner.Spawn(bullet, transform.position, inputAuthority: Object.InputAuthority,
                onBeforeSpawned: (_, bulletObj) =>
                {
                    bulletObj.transform.rotation.SetLookRotation(obj.transform.position);
                    var angles = bulletObj.transform.rotation.eulerAngles;
                    bulletObj.transform.rotation = Quaternion.Euler(0, angles.y, 0);
                });
        }
    }
}
