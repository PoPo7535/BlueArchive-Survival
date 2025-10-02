using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class MonsterHitManager : MonsterComponent
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
                HitUtile.ShowHitLog(hitData, transform);
        }
        _previousHitCount = _hitCount;
    }

    public void Damage(PlayerRef player, float damage)
    {
        _hitData.Set(_hitCount % HitCapacity, new HitData()
        {
            other = player,
            damage = damage,
            critical = false,
        });
        ++_hitCount;
    }

    public override void Init(MonsterBase monster)
    {
        base.Init(monster);
        monster.HitManager = this;
    }
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        _hitCount = 0;
        _previousHitCount = 0;
    }
}


