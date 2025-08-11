using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class ActiveSkillBase : SkillBase, IActiveSkill
{
    protected PlayerBase player;
    protected PlayerSkillInventory skillInventory;
    public void Init(PlayerSkillInventory skillInventory)
    {
        this.skillInventory = skillInventory;
    }
    
    public override void Spawned()
    {
        base.Spawned();
        if (false == Object.HasStateAuthority)
            return;
        player = App.I.GetPlayerInfo(Object.InputAuthority).PlayerObject.GetComponent<PlayerBase>();
    }
    public virtual void StatusUpdate() { }
}