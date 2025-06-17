using UnityEngine;

public class FsmPlayerMove : FsmPlayerBase
{
    public override void OnUpdate(Spawner.NetworkInputData data)
    {
        if (_other.CanAttack())
        {
            _other.ChangeState(FsmState.Attack);
            return;
        }
        if (data.input == Vector2.zero)
        {
            _other.ChangeState(FsmState.Idle);
            return;
        }
        _other.Move(data);
        _other.Rotation(data);
        ani.SetTrigger(StringToHash.Move);   
    }
    public override void OnExit()
    {
        ani.ResetTrigger(StringToHash.Move);
    }
}