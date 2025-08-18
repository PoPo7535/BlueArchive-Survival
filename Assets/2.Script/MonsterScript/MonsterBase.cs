using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class MonsterBase : NetworkBehaviour
{
    [NonSerialized, Read] public MonsterFsmController FsmController;
    [NonSerialized, Read] public MonsterHitData HitData;
    [NonSerialized, Read] public MonsterDamage Damage;
    private void Awake()
    {
        foreach (var component in GetComponents<MonoBehaviour>())
        {
            if (component is IMonsterComponent monsterComponent)
                monsterComponent.Init(this);
        }
    }
}
