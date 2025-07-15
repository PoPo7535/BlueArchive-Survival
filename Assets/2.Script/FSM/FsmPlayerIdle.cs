using UnityEngine;

public class FsmPlayerIdle : FsmPlayerBase
{
    public FsmPlayerIdle(IFsmStateOther other) : base(other)
    {
    }

    public override void OnUpdate(NetworkInputData data)
    {
        if (other.CanAttack())
        {
            other.ChangeState(FsmState.Attack);
            return;
        }
        if (data.dir != Vector2.zero)
        {
            other.ChangeState(FsmState.Move);
            return;
        }

        _controller.AniTrigger = StringToHash.Idle;
        // ani.SetTrigger(StringToHash.Idle);
    }

    public override void OnExit()
    {
        // ani.ResetTrigger(StringToHash.Idle);
    }
}
