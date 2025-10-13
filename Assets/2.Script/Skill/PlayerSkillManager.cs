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
    [Read] private readonly Dictionary<SkillType, ActiveSkillBase> _activeSkills = new();
    [Read] private readonly Dictionary<SkillType, ActiveSkillBase> _passiveSkills = new();
    [Read] private readonly Dictionary<SkillType, int> _probabilitySkills = new();
    [Read] private SkillType[] _skillTypes = new SkillType[3]; 
    public override void Init(PlayerBase player)
    {
        base.Init(player);
        Player.SkillManager = this;
        foreach (var skillType in SkillType.None.ToArray().Skip(1))
            _probabilitySkills.Add(skillType, 1);
    }

    public override void Spawned()
    {
        base.Spawned();
        Random.InitState(Object.InputAuthority.PlayerId + Runner.Tick);
    }

    public void AddSkill(ActiveSkillBase skillBase, SkillType type)
    {
        if (false == _activeSkills.TryAdd(type, skillBase))
        {
            $"{nameof(AddSkill)} Error".ErrorLog();
            return;
        }
        SkillUpdate();
    }

    public void RemoveProbability(SkillType type)
    {
        _probabilitySkills.Remove(type);
    }
    public void SkillUpdate()
    {
        CharacterSkillPanel.I.SkillUpdate(_activeSkills);
    }

    public SkillType GetRandomSkill()
    {
        var skillTypes = _probabilitySkills.Keys.ToList();
        foreach (var skillType in skillTypes)
            _probabilitySkills[skillType] += 1;
        
        skillTypes = _activeSkills.Keys.Where(skillType => _activeSkills.ContainsKey(skillType)).ToList();
        foreach (var type in skillTypes)
        {
            if (_probabilitySkills.ContainsKey(type))
                _probabilitySkills[type] += 3;
        }

        var sum = _probabilitySkills.Keys.Sum(skillType => _probabilitySkills[skillType]);
        var randomValue = Random.Range(1, sum);
        var current = 0;

        var output = string.Empty;
        foreach (var pair in _probabilitySkills)
            output += $"[{pair.Key}:{pair.Value}] ";
        $"{output} = {randomValue}".Log();
        
        foreach (var pair in _probabilitySkills)
        {
            current += _probabilitySkills[pair.Key];
            if (randomValue <= current)
            { 
                _probabilitySkills[pair.Key] = 0;
                return pair.Key;
            }
        }

        $"{nameof(GetRandomSkill)} 확률버그".WarningLog();
        return SkillType.None;
    }

    public void Foo()
    {
        for (int i = 0; i < _skillTypes.Length; ++i)
            _skillTypes[i] = GetRandomSkill();
        
    }

    public SkillType GetSkillType(int index) => _skillTypes[index];
    
    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_GetSkill(SkillType skillType, RpcInfo info = default)
    {
        if (_activeSkills.ContainsKey(skillType))
        {
            if (_activeSkills[skillType].CanLevelUp) 
            {
                _activeSkills[skillType].LevelUp();
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
