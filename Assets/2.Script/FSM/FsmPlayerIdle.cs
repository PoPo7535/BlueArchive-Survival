using UnityEngine;

public class FsmPlayerIdle : FsmPlayerBase
{
    public override void OnUpdate(Spawner.NetworkInputData data)
    {
        if (_other.CanAttack())
        {
            _other.ChangeState(FsmState.Attack);
            return;
        }
        if (data.input != Vector2.zero)
        {
            _other.ChangeState(FsmState.Move);
            return;
        }
        ani.SetTrigger(StringToHash.Idle);
    }

    public override void OnExit()
    {
        ani.ResetTrigger(StringToHash.Idle);
    }
}
