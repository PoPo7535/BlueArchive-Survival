using System.Collections;
using System.Collections.Generic;
using Fusion;
using Sirenix.Serialization;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class PlayerSkillManager : PlayerComponent
{
    [Read] private Dictionary<SkillType, ActiveSkillBase> activeSkills = new();
    // [Read] private List<IPassiveSkill> passiveSkills = new();
    public override void Init(PlayerBase player)
    {
        base.Init(player);
        Player.SkillManager = this;
    }

    public void AddSkill(ActiveSkillBase skillBase, SkillType type)
    {
        activeSkills.Add(type, skillBase);
    }

    public void UpdateSkillData()
    {
        
    }
    public void GetSkill()
    {
        
    }
    
    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SkillSpawn(RpcInfo info = default)
    {
        if (false == Object.HasStateAuthority)
            return;
        Runner.Spawn(
            GameManager.I.skillDataTests[SkillType.Wheel].skillObj, 
            Vector3.zero, 
            Quaternion.identity, 
            info.Source);

    }
}
