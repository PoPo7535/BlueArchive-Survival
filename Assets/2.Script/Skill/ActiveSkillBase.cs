using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class ActiveSkillBase : SkillBase, IActiveSkill
{
    protected PlayerSkillInventory skillInventory;
    public void Init(PlayerSkillInventory skillInventory)
    {
        this.skillInventory = skillInventory;
    }
    public virtual void StatusUpdate() { }
}