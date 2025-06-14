using System;
using System.Collections;
using System.Collections.Generic;
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

    [Serial, Read] public Rigidbody rigi;
    public GameObject bullet;
    [Fold("State")] public float moveSpeed = 30;
    [Fold("State")] public float rotateSpeed = 30;
    [Fold("State")] private float attackDelay = 0;
    [Fold("State")] private float attackDelayMax = 2f;

    [Button, GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        rigi = GetComponent<Rigidbody>();
    }


    public override void Init(PlayerBase player)
    {
        base.Init(player);
        PB.fsmController = this;
        
        _idleStateTarget = new FsmPlayerIdle();
        _moveStateTarget = new FsmPlayerMove();
        
        _idleStateTarget.OnInit(this);
        _moveStateTarget.OnInit(this);
        // _attackStateTarget.OnInit(this);
        // _skillStateTarget.OnInit(this);

        _currentStateTarget = _idleStateTarget;
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out Spawner.NetworkInputData data))
        {
            _currentStateTarget.OnUpdate(data);
        }
    }

    public void ChangeState(FsmState newState)
    {
        _currentStateTarget.OnExit();
        switch (newState)
        {
            case FsmState.Idle:
                _currentStateTarget = _idleStateTarget;
                break;
            case FsmState.Move:
                _currentStateTarget = _moveStateTarget;
                break;
        }
        _currentStateTarget.OnEnter();
    }


    public void Move(Spawner.NetworkInputData data)
    {
        var dir = data.input.normalized * (moveSpeed * Runner.DeltaTime);

        if (data.input is { x: 0, y: 0 })
        {
            rigi.velocity = new Vector3(0, rigi.velocity.y, 0); 
            return; 
        }
        rigi.velocity = new Vector3(dir.x,  rigi.velocity.y, dir.y);
    }

    public void Rotation(Spawner.NetworkInputData data)
    {
        var dir = data.input.normalized * (moveSpeed * Runner.DeltaTime);
        attackDelay += Runner.DeltaTime;
        if (false == PB.findEnemy.nearObj.IsUnityNull() && attackDelayMax < attackDelay) 
        {
            // Shot();
            attackDelay = 0;
            Runner.Spawn(bullet, transform.position + (transform.forward + Vector3.up) * 0.5f, transform.rotation);
            return;
        }
        
        if (data.input is { x: 0, y: 0 })
            return;
        
        var targetRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.y));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Runner.DeltaTime);
    }
    
    private void Shot() 
    {
        // var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        // var plane = new Plane(Vector3.up, transform.position);
        // if (false == plane.Raycast(ray, out var distance)) 
        //     return;
        // var targetPoint = ray.GetPoint(distance);
        
        var targetPoint = PB.findEnemy.nearObj.transform.position;
        var dir = (targetPoint - transform.position).normalized;
        var lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10000);
    }
    
}

public enum FsmState
{
    Idle,
    Move,
}
