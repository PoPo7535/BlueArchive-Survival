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
    [Read] private HashSet<SkillType> activeSkills = new();
    // [Read] private List<IPassiveSkill> passiveSkills = new();
    public override void Init(PlayerBase player)
    {
        base.Init(player);
        Player.SkillManager = this;
    }

    public void AddSkill(SkillType type)
    {
        activeSkills.Add(type);
    }
        
    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SkillSpawn(RpcInfo info = default)
    {
        if (false == Object.HasStateAuthority)
            return;
        Runner.Spawn(GameManager.I.skillDataTests[SkillType.Wheel].skillObj, 
            Vector3.zero, Quaternion.identity, info.Source);
        // Runner.Spawn(GameManager.I.skillDataTests[SkillType.Wheel].skillObj, 
        //     Vector3.zero, Quaternion.identity, info.Source);
    }
}
