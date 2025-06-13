using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class PlayerStateController : PlayerComponent
{
    private IPlayerCharacterState _idleState;
    private IPlayerCharacterState _moveState;
    private IPlayerCharacterState _attackState;
    private IPlayerCharacterState _skillState;
    
    
    private IPlayerCharacterState _movementState;
    private IPlayerCharacterState _actionState;

    public enum TestState
    {
        Movement,
        Action,
    }
    public override void Init(PlayerBase player)
    {
        base.Init(player);
        PB.playerStateController = this;
        
        _idleState.OnInit(PB);
        _moveState.OnInit(PB);
        _attackState.OnInit(PB);
        _skillState.OnInit(PB);
    }

    public void ChangedState(TestState state, IPlayerCharacterState newState)
    {
        switch (state)
        {
            case TestState.Movement:
                _movementState.OnExit();
                _movementState = newState;
                _movementState.OnEnter();
                break;
            case TestState.Action:
                _actionState.OnExit();
                _actionState = newState;
                _actionState.OnEnter();
                break;
        }
    }
    public override void FixedUpdateNetwork()
    {
        _movementState.OnUpdate();
        _actionState.OnUpdate();
    }
}
