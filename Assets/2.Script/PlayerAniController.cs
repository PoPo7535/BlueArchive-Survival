using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class PlayerAniController : PlayerComponent
{
    [Read] private Animator ani;
    [Networked] private int aniTrigger { get; set; }

    public override void Init(PlayerBase player)
    {
        base.Init(player);
        PB = player;
        PB.aniController = this;
    }
    public override void FixedUpdateNetwork()
    {
        if (false == HasStateAuthority)
            return;
        ani.SetTrigger(aniTrigger);
    }

    public void ASD()
    {
    }
}
