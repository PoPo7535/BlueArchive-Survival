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
        player.state = this;
    }
    public void Start()
    {
        var controller = Player.aniController.ani.runtimeAnimatorController;
        var ac = controller.animationClips.FirstOrDefault
            (c => c.name.Split('_')[1] == nameof(StringToHash.Attack));
        
        _attackBaseSpeed = ac.length / 2f;
        UpdateAniSpeed();
    }
    public void Damage(int damage)
    {
        hp -= damage;
    }

    private void UpdateAniSpeed()
    {
        Player.aniController.ChangeSpeed(FsmState.Move, _moveSpeed / 100f);
        Player.aniController.ChangeSpeed(FsmState.Attack,AttackSpeed / 100f);
    }
}