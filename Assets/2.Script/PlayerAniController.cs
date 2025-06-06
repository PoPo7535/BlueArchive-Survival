using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class PlayerAniController : PlayerComponent
{
    public PlayerAni playerAni;
    public Animator ani => playerAni.ani;
    private int aniTrigger { get; set; }

    public override void Init(PlayerBase player)
    {
        base.Init(player);
        PB = player;
        PB.aniController = this;
    }

    public void SetTrigger(int aniTrigger)
    {
        this.aniTrigger = aniTrigger;
    }
    public void LateUpdate()
    {
        aniTrigger.Log();
        ani.SetTrigger(aniTrigger);
    }
}
