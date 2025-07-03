using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Sirenix.OdinInspector;
using UnityEngine;
using Utility;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public partial class BattleSceneManager : LocalFusionSingleton<BattleSceneManager>, IPlayerLeft, ISetInspector
{
    [Networked, OnChangedRender(nameof(OnChangedExp))] public float exp { get; set; }
    public BattleData battleData = new();
    private ExpBar expBar;
    [Read, Serial] private const float ExpUpdateMaxTime = 1f;
    [Read, Serial] private AbilitySelectPanel _selectPanel;
    [Read, Serial] private float _expUpdateTime = 0f;
    [Read, Serial] private float _levelUpValue = 100f;

    [Button, GUIColor(0,1f,0)]
    public void SetInspector()
    {
    }
    
    private void Start()
    {
        ReadyInit();
    }

    public override void FixedUpdateNetwork()
    {
        UpdateExp();
    }
    private void UpdateExp()
    {
        _expUpdateTime += Runner.DeltaTime;
        if (_expUpdateTime < ExpUpdateMaxTime)
            return;
        _expUpdateTime = 0f;
        if (0f < battleData.curExp)
        {
            RPC_AddExp(battleData.curExp);
            battleData.ReSetExp();
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    private void RPC_AddExp(float addExp)
    {
        exp += addExp;
    }

    private void OnChangedExp()
    {
        var expValue = exp / _levelUpValue;
        expBar.SetBar(expValue);
        if (1f <= expValue)
            LevelUp();
    }

    private void LevelUp()
    {
        exp -= _levelUpValue;
    }


}
