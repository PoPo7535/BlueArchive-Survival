using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class PlayerSkillManager : PlayerComponent
{
    [Read] private Dictionary<SkillType, ActiveSkillBase> activeSkills = new();
    [Read] private Dictionary<SkillType, int> randomSkillValue = new();
    public override void Init(PlayerBase player)
    {
        base.Init(player);
        Player.SkillManager = this;
        foreach (var skillType in SkillType.None.ToArray().Skip(1))
            randomSkillValue.Add(skillType, 1);
    }

    public override void Spawned()
    {
        base.Spawned();
        Random.InitState(Object.InputAuthority.PlayerId + Runner.Tick);
    }

    public void AddSkill(ActiveSkillBase skillBase, SkillType type)
    {
        if (false == activeSkills.TryAdd(type, skillBase))
            $"{nameof(AddSkill)} Error".ErrorLog();
    }

    public SkillType GetRandomSkill()
    {
        var skillTypes = randomSkillValue.Keys.ToList();
        foreach (var skillType in skillTypes)
            randomSkillValue[skillType] += 1;
        
        skillTypes = activeSkills.Keys.Where(skillType => activeSkills.ContainsKey(skillType)).ToList();
        foreach (var skillType in skillTypes)
            randomSkillValue[skillType] += 3;

        var sum = randomSkillValue.Keys.Sum(skillType => randomSkillValue[skillType]);
        var randomValue = Random.Range(0, sum);
        var current = 0;

        foreach (var pair in randomSkillValue)
        {
            current += randomSkillValue[pair.Key];
            if (pair.Key.IsLast())
            {
                randomSkillValue[pair.Key] = 0;
                return pair.Key;
            }
            var max = current + randomSkillValue[pair.Key.Next()];
            if (current <= randomValue && randomValue < max)
            {
                randomSkillValue[pair.Key] = 0;
                return pair.Key;
            }
        }

        $"{nameof(GetRandomSkill)} 확률버그".WarningLog();
        return SkillType.None;
    }
    
    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_GetSkill(SkillType skillType, RpcInfo info = default)
    {

        if (activeSkills.ContainsKey(skillType))
        {
            if (activeSkills[skillType].CanLevelUp) 
            {
                activeSkills[skillType].LevelUp();
                return;
            }
            $"{nameof(RPC_GetSkill)} 오류".ErrorLog();
            return;
        }
        if (false == Object.HasStateAuthority)
            return;
        Runner.Spawn(
            GameManager.I.GetSkillScriptable(skillType).skillObj, 
            Vector3.zero, 
            Quaternion.identity, 
            info.Source);
    }
}
