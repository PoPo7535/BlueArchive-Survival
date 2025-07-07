using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class PlayerState : PlayerComponent
{
    public int hp = 100;
    public int maxHp = 100;
    public int defensive;
    public float attackSpeed = 100f;
    public float moveSpeed = 100f;
    public override void Init(PlayerBase player)
    {
        base.Init(player);
        player.state = this;
    }
    
    public void Damage(int damage)
    {
        hp -= damage;
    }

    [Button]
    public void LevelUp(float val)
    {
        Player.ani.ChangeSpeed(FsmState.Attack,attackSpeed / 100f);
        Player.ani.ChangeSpeed(FsmState.Move, moveSpeed / val);
    }
}
