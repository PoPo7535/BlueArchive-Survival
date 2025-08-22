using System.Collections;
using System.Collections.Generic;using Fusion;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class MonsterComponent : NetworkBehaviour, IMonsterComponent
{
    [Read] public MonsterBase Monster { get; private set; }

    public virtual void Init(MonsterBase monster)
    {
        Monster = monster;
    }
}
