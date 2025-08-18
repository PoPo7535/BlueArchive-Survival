using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class MonsterHitData : NetworkBehaviour ,IMonsterComponent
{
    private const int HitCapacity = 5;
    [Networked, Capacity(HitCapacity)] private NetworkArray<HitData> _hitData { get; }
    [Networked] private int _hitCount { get; set; }
    private int _previousHitCount;

    public override void Render() 
    {
        if (_previousHitCount == _hitCount)
            return;
        for (int i = 0; i < _hitCount - _previousHitCount; ++i)
        {
            var index = (_previousHitCount + i) % HitCapacity;
            var hitData = _hitData.Get(index);
            if (Runner.LocalPlayer == hitData.other)
                HitUtile.ShowHitLog(_hitData.Get((_previousHitCount + i) % HitCapacity), transform);
        }
        _previousHitCount = _hitCount;
    }

    public void Damage(PlayerRef player, int damage)
    {
        _hitData.Set(_hitCount % HitCapacity, new HitData()
        {
            other = player,
            damage = _hitCount,
        });
        ++_hitCount;
    }

    public void Init(MonsterBase monsterBase)
    {
        monsterBase.HitData = this;
    }
}


