using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Animations;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class ActiveSkillTest : ActiveSkillBase, ISetInspector
{
    [Read, Serial] public PositionConstraint positionConstraint;
    [Button, GUIColor(0, 1, 0)]
    public new void SetInspector()
    {
        base.SetInspector();
        positionConstraint = GetComponent<PositionConstraint>();
    }

    public override void Spawned()
    {
        base.Spawned();
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        positionConstraint.AddSource(new ConstraintSource()
        {
            sourceTransform = player.transform,
            weight = 1,
        });
        foreach (var trigger in activeSkills)
            trigger.Init(player, TriggerType.Enter);
        
    }
}
