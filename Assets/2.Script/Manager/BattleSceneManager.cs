using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Utility;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class BattleSceneManager : LocalFusionSingleton<BattleSceneManager>
{
    [Networked, OnChangedRender(nameof(OnChangedExp))] public float exp { get; set; }
    
    public BattleData battleData = new();
    public ExpBar expBar;
    private float _expUpdateTime = 0f;
    public void Test()
    {
        battleData.AddExp(3f);
    }
    public override void FixedUpdateNetwork()
    {
        UpdateExp();
    }
    private void UpdateExp()
    {
        _expUpdateTime += Runner.DeltaTime;
        if (_expUpdateTime < 1f)
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
        expBar.SetBar(exp / 100f);
        if (exp < 100)
            return;
    }
}
