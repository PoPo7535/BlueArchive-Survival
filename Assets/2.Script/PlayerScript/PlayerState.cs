using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private float _moveSpeed = 100f;
    public float MoveSpeed
    {
        get => _moveSpeed;
        set
        {
            _moveSpeed = value;
            UpdateAniSpeed();
        }
    }
    
    private float _attackValue = 1f;
    private float _attackSpeed = 100f;
    private float _attackBaseSpeed;
    private float AttackSpeed
    {
        get => _attackSpeed * _attackBaseSpeed;
        set
        {
            _attackSpeed = value;
            UpdateAniSpeed();
        }
    }

    public override void Init(PlayerBase player)
    {
        base.Init(player);
        player.State = this;
    }
    public void Start()
    {
        var controller = Player.AniController.ani.runtimeAnimatorController;
        var ac = controller.animationClips.FirstOrDefault
            (c => c.name.Split('_')[1] == nameof(StringToHash.Attack));
        
        _attackBaseSpeed = ac.length / 2f;
        UpdateAniSpeed();
    }
    public void HitDamage(int damage)
    {
        hp -= damage;
    }

    public float GetDamageValue(float damage) => damage * _attackValue;

    private void UpdateAniSpeed()
    {
        Player.AniController.ChangeSpeed(FsmState.Move, _moveSpeed / 100f);
        Player.AniController.ChangeSpeed(FsmState.Attack,AttackSpeed / 100f);
    }
}