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

public class PlayerAniController : PlayerComponent
{
    [Serial, Read] public PlayerAni playerAni;
    public Animator ani => playerAni.ani;

    private Dictionary<int, float> dic = new();
    public override void Init(PlayerBase player)
    {
        base.Init(player);
        PB.aniController = this;
    }

    public void Start()
    {
        var controller = ani.runtimeAnimatorController;
        foreach (var clip in controller.animationClips)
        {
            dic.Add(Animator.StringToHash(clip.name.Split('_')[1]), 1f);
        }
        var ac = controller.animationClips.FirstOrDefault
            (c => c.name.Split('_')[1] == nameof(StringToHash.Attack));
        dic[StringToHash.Attack] = ac.length > 0 ? 1f / ac.length : 1.0f;
        dic[StringToHash.Attack].Log();
    }

    public void ChangeSpeed(int stringHash)
    {
        ani.speed = dic[stringHash];
    }
}
