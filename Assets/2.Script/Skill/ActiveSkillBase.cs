using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class ActiveSkillBase : SkillBase, IActiveSkill
{
    protected PlayerSkillInventory SkillInventory;
    public void Init(PlayerSkillInventory skillInventory)
    {
        this.SkillInventory = skillInventory;
    }
    public virtual void StatusUpdate() { }
}