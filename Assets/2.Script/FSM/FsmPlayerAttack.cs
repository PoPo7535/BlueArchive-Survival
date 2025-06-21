using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class FsmPlayerAttack : FsmPlayerBase
{
    private bool doAttack = true;
    public override void OnInit(IFsmStateOther other)
    {
        base.OnInit(other);
        _controller.attackAction = Shot;
    }

    public override void OnEnter()
    {
        doAttack = true;
        ani.SetTrigger(StringToHash.Attack);   
        ani.Update(0f);
    }
    public override void OnUpdate(Spawner.NetworkInputData data)
    {
        if (doAttack)
            RotateFromTarget();
        
        var stateInfo = ani.GetCurrentAnimatorStateInfo(0); 
        if (stateInfo.shortNameHash != StringToHash.Attack)
        {
            _other.ChangeState(FsmState.Idle);
        }

        _other.Move(data);
        // _other.Rotation(data);
    }

    private void RotateFromTarget()
    {
        var tr = _controller.transform;
        var targetPoint = _controller.Player.findEnemy.nearObj.transform.position;
        var dir = (targetPoint - tr.position).normalized;
        var lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        tr.rotation = Quaternion.Slerp(tr.rotation, lookRotation, 10000);
    }
    private void Shot()
    {
        doAttack = false;
        var tr = _controller.transform;
        _controller.attackDelay = 0;
        _controller.Runner.Spawn(_controller.bullet, tr.position + (tr.forward + Vector3.up) * 0.5f, tr.rotation);
    }
}
