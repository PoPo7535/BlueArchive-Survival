using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Animations;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class SkillTriggerTest : SkillTriggerBase, ISetInspector
{
    [Read, Serial] public PositionConstraint positionConstraint;
    [Button, GUIColor(0, 1, 0)]
    public void SetInspector()
    {
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
        
    }
}
