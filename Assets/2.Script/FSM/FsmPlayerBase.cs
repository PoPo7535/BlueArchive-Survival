using Unity.VisualScripting;
using UnityEngine;

public class FsmPlayerBase : IFsmStateTarget
{
    protected IFsmStateOther _other;
    protected PlayerFsmController _controller;
    protected Animator ani => _controller.Player.ani.ani;
    public virtual void OnInit(IFsmStateOther other)
    {
        _other = other;
        _controller = other.GetComponent<PlayerFsmController>();
    }
    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void OnUpdate(Spawner.NetworkInputData data) { }
}
