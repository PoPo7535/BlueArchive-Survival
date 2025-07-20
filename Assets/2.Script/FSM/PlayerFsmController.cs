using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class PlayerFsmController : PlayerComponent, IFsmStateOther, ISetInspector
{
    private IFsmStateTarget _idleStateTarget;
    private IFsmStateTarget _moveStateTarget;
    private IFsmStateTarget _attackStateTarget;
    private IFsmStateTarget _skillStateTarget;

    private IFsmStateTarget _currentStateTarget;

    public GameObject bullet;
    [Serial, Read] public Rigidbody rigi;
    [Fold("State")] public float rotateSpeed = 30;
    [Fold("State")] public float attackDelay = 0;
    [Fold("State")] public float attackDelayMax = 2f;
    public Action attackAction;

    [Networked, OnChangedRender(nameof(SetAni))]
    public int AniTrigger { get; set; }

    public void SetAni(NetworkBehaviourBuffer previous)
    {
        var prevValue = GetPropertyReader<int>(nameof(AniTrigger)).Read(previous);
        Player.aniController.ani.ResetTrigger(prevValue);
        Player.aniController.ani.SetTrigger(AniTrigger);
        if (StringToHash.Attack != AniTrigger)
            Player.aniController.ani.Update(0f);
    }
    
    [Button, GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        rigi = GetComponent<Rigidbody>();
    }

    public override void Spawned()
    {
        AniTrigger = StringToHash.Idle;
    }

    public override void Init(PlayerBase player)
    {
        base.Init(player);
        Player.fsmController = this;
        
        _idleStateTarget = new FsmPlayerIdle(this);
        _moveStateTarget = new FsmPlayerMove(this);
        _attackStateTarget = new FsmPlayerAttack(this);
        // _skillStateTarget.OnInit(this);

        _currentStateTarget = _idleStateTarget;
        _currentStateTarget.OnEnter();
    }


    public override void FixedUpdateNetwork()
    {
        if (false == HasStateAuthority)
            return;
        
        attackDelay += Runner.DeltaTime;
        if (GetInput(out NetworkInputData data)) 
            _currentStateTarget.OnUpdate(data);

    }
    public void LateUpdate()
    {
        if (StringToHash.Attack != AniTrigger)
            Player.aniController.ani.SetTrigger(AniTrigger);
    }

    public void ChangeState(FsmState newState)
    {
        _currentStateTarget.OnExit();
        _currentStateTarget = newState switch
        {
            FsmState.Idle => _idleStateTarget,
            FsmState.Move => _moveStateTarget,
            FsmState.Attack => _attackStateTarget,
            _ => _currentStateTarget
        };
        _currentStateTarget.OnEnter();
    }

    public void OnAttack()
    {
        attackAction?.Invoke();
    }

    public void Move(NetworkInputData data, Transform _)
    {
        var dir = data.dir.normalized * (Player.state.MoveSpeed * Runner.DeltaTime);
        rigi.velocity = new Vector3(dir.x,  rigi.velocity.y, dir.y);
    }

    public void Rotation(NetworkInputData data, Transform _)
    {
        var dir = data.dir.normalized * (Player.state.MoveSpeed * Runner.DeltaTime);
        var targetRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.y));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Runner.DeltaTime);
    }

    public bool CanAttack()
    {
        if (false == Player.HasStateAuthority)
            return false;
        return false == Player.findEnemy.nearObj.IsUnityNull() && attackDelayMax < attackDelay;
    }
}

public enum FsmState
{
    Idle,
    Move,
    Attack,
}
