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
    [Serial, Read] public Rigidbody rigi;
    [Serial, Read] public Animator ani;
    public GameObject expObj;
    
    private IFsmStateTarget _moveStateTarget;
    private IFsmStateTarget _currentStateTarget;

    public float moveSpeed = 100f;
    public float rotateSpeed = 100f;
    
    [Button, GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        rigi = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
    }

    public void Start()
    {
        _moveStateTarget = new FsmMonsterMove(this);
        _currentStateTarget = _moveStateTarget;
        _currentStateTarget.OnEnter();
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
        _currentStateTarget.OnEnter(); 
    }

    public void Move(NetworkInputData _, Transform target)
    {
        ani.SetTrigger(StringToHash.Move);
        if (false == HasStateAuthority)
            return;
        var dir = (target.position - transform.position).normalized *
                  (moveSpeed * Runner.DeltaTime);
        rigi.velocity = new Vector3(dir.x, rigi.velocity.y, dir.z);
    }

    public void Rotation(NetworkInputData _ , Transform target)
    {
        var dir = (target.position - transform.position).normalized *
                  (moveSpeed * Runner.DeltaTime);
        var targetRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Runner.DeltaTime);
    }

    public bool CanAttack()
    {
        return true;
    }
    public void Damage(PlayerRef other = default)
    {
        Instantiate(expObj, transform.position, Quaternion.identity);
        Runner.Despawn(Object);
    }
}
