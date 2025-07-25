using Unity.VisualScripting;
using UnityEngine;

public class FsmPlayerBase : FsmBase
{
    protected PlayerFsmController _controller;
    protected Animator ani => _controller.Player.aniController.ani;

    public FsmPlayerBase(IFsmStateOther other) : base(other)
    {
        _controller = other.GetComponent<PlayerFsmController>();
    }
}
