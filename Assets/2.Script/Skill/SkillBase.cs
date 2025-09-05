using System.Collections;
using System.Collections.Generic;
using Fusion;
using Sirenix.OdinInspector;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class SkillBase : NetworkBehaviour
{
    protected SkillData skillData;
    [Read, Fold("State")] public int maxLevel = 1;
    [Read, Fold("State")] public int currentLevel = 1;
}
