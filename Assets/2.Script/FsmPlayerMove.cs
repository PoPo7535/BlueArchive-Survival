using UnityEngine;

public class FsmPlayerMove : IFsmStateTarget
{
    private IFsmStateOther _other;
    private PlayerFsmController _controller;
    public void OnInit(IFsmStateOther other)
    {
        _other = other;
        _controller = other.GetComponent<PlayerFsmController>();
    }

    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate(Spawner.NetworkInputData data)
    {
        if (data.input == Vector2.zero)
        {
            _other.ChangeState(FsmState.Idle);
            return;
        }
        _other.Move(data);
        _other.Rotation(data);
        _controller.PB.aniController.SetTrigger(StringToHash.Move);
    }

}