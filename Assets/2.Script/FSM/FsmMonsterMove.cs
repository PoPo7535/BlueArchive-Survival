using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class FsmMonsterMove : FsmMonsterBase
{
    public FsmMonsterMove(IFsmStateOther other) : base(other)
    {
    }

    public override void OnUpdate(Spawner.NetworkInputData _)
    {
        
    }
}
