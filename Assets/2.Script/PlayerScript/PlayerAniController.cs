using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;
using Random = UnityEngine.Random;

public class PlayerAniController : PlayerComponent
{
    [Serial, Read] public PlayerAni playerAni;
    public Animator ani => playerAni.ani;

    public override void Init(PlayerBase player)
    {
        base.Init(player);
        Player.ani = this;
    }

    public void Start()
    {
    }

    public void ChangeSpeed(FsmState state, float speed = 1f)
    {
        switch (state)
        {
            case FsmState.Attack:
                ani.SetFloat(StringToHash.AttackSpeed,3f);
                break;
            case FsmState.Move:
                ani.SetFloat(StringToHash.MoveSpeed, speed);
                break;
            case FsmState.Idle:
                break;
        }
        
    }
}
