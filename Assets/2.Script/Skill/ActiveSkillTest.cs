using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class ActiveSkillTest : ActiveSkillBase
{
    public override void Spawned()
    {
        base.Spawned();
        if (false == Object.HasStateAuthority)
            return;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public override void StatusUpdate()
    {
        
    }
}
