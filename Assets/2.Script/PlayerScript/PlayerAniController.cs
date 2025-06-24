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

    private Dictionary<FsmState, float> dic = new();
    public override void Init(PlayerBase player)
    {
        base.Init(player);
        Player.ani = this;
    }

    public void Start()
    {
        var controller = ani.runtimeAnimatorController;
        foreach (var clip in FsmState.Idle.ToArray())
        {
            dic.Add(clip, 1f);
        }
        
        var ac = controller.animationClips.FirstOrDefault
            (c => c.name.Split('_')[1] == nameof(StringToHash.Attack));
        dic[FsmState.Attack] = ac.length / 2f;
    }

    public void ChangeSpeed(FsmState state, float speed = 1f)
    {
        ani.speed = dic[state] * speed;
    }
}
