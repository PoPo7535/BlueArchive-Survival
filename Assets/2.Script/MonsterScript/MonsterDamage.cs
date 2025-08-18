using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class MonsterDamage : NetworkBehaviour , IMonsterComponent
{
    [Networked, Capacity(3), OnChangedRender(nameof(OnAttackerLog))] private NetworkDictionary<PlayerRef, bool> attackerLog { get; }
    private Queue<PlayerRef> attackerQueue = new();
    public ExpObject expObj;
    public float dropExpValue = 40f;
    private void OnAttackerLog()
    {
        if (attackerLog[Runner.LocalPlayer])
        {
            var number = attackerLog.Count(pair => pair.Value);
            var expObject = Instantiate(expObj, transform.position, Quaternion.identity);
            expObject.Init(dropExpValue / number);
        }

        if (attackerLog.Any(pair => pair.Value))
            Runner.Despawn(Object);
    }

    public override void FixedUpdateNetwork()
    {
        if (BattleSceneManager.I.PauseObject)
            return;
        while (0 != attackerQueue.Count)
            attackerLog.Set(attackerQueue.Dequeue(), true);
    }
    
    public override void Spawned()
    {
        attackerLog.Clear();
        foreach (var playerRef in App.I.GetAllPlayers())
            attackerLog.Add(playerRef, false);
    }
    public override void  Despawned(NetworkRunner runner, bool hasState)
    {
    }
    public void Damage(PlayerRef other = default)
    {
        if (false == attackerLog[other])
            attackerQueue.Enqueue(other);
    }

    public void Init(MonsterBase monsterBase)
    {
        monsterBase.Damage = this;
    }
}
