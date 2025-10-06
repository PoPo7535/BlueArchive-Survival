using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class MonsterDamage : MonsterComponent
{
    public ExpObject expObj;
    public float dropExpValue = 40f;
    private void OnAttackerLog()
    {
        // if (attackerLog[Runner.LocalPlayer])
        // {
        //     var number = attackerLog.Count(pair => pair.Value);
        //     var expObject = Instantiate(expObj, transform.position, Quaternion.identity);
        //     expObject.Init(dropExpValue / number);
        // }
        //
        // if (attackerLog.Any(pair => pair.Value))
        //     Runner.Despawn(Object);
    }

    public void Damage(PlayerRef other = default)
    {

    }

    public override void Init(MonsterBase monster)
    {
        base.Init(monster);
        monster.Damage = this;
    }
}
