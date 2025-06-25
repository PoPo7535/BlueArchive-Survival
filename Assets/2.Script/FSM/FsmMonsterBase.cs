using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class FsmMonsterBase : FsmBase
{
    public FsmMonsterBase(IFsmStateOther other) : base(other)
    {
    }
}
