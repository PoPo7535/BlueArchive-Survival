using UnityEngine;

public class FsmPlayerMove : FsmPlayerBase
{
    public FsmPlayerMove(IFsmStateOther other) : base(other)
    {
    }

    public override void OnUpdate(Spawner.NetworkInputData data)
    {
        if (other.CanAttack())
        {
            other.ChangeState(FsmState.Attack);
            return;
        }
        if (data.input == Vector2.zero)
        {
            other.ChangeState(FsmState.Idle);
            return;
        }
        other.Move(data);
        other.Rotation(data);
        ani.SetTrigger(StringToHash.Move);   
    }
    public override void OnExit()
    {
        ani.ResetTrigger(StringToHash.Move);
    }
}