using System.Collections;
using System.Collections.Generic;
using Fusion;
using Sirenix.OdinInspector;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class MonsterFsmController : NetworkBehaviour, IFsmStateOther, ISetInspector
{
    [Serial, Read] private Rigidbody rigi;
    [Serial, Read] private Animator ani;
    [Button, GUIColor(0, 1, 0)]
    
    
    private IFsmStateTarget _moveStateTarget;
    private IFsmStateTarget _currentStateTarget;

    public void SetInspector()
    {
        rigi = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
    }

    public void Start()
    {
        _moveStateTarget = new FsmMonsterMove(this);

        _currentStateTarget = _moveStateTarget;
    }
    public override void FixedUpdateNetwork()
    {
        _currentStateTarget.OnUpdate(default);
    }
    public void ChangeState(FsmState newState)
    {
        _currentStateTarget.OnExit();
        _currentStateTarget = newState switch
        {
            FsmState.Move => _moveStateTarget,
            _ => _currentStateTarget
        };
        _currentStateTarget.OnEnter(); // 오늘치 집중력이 다 떨어졌어
    }

    public void Move(Spawner.NetworkInputData data)
    {
    }

    public void Rotation(Spawner.NetworkInputData data)
    {
    }

    public bool CanAttack()
    {
        return true;
    }
}
