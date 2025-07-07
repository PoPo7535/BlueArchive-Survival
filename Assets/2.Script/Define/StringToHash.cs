using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringToHash
{
    public static readonly int Idle = Animator.StringToHash("Idle");
    public static readonly int Move = Animator.StringToHash("Move");
    public static readonly int Attack = Animator.StringToHash("Attack");
    public static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");
    public static readonly int AttackSpeed = Animator.StringToHash("AttackSpeed");
    public static readonly int Formation_Idle = Animator.StringToHash("Formation Idle");

    public static readonly Dictionary<FsmState, int> StateHash = new()
    {
        { FsmState.Idle, Idle },
        { FsmState.Move, Move },
        { FsmState.Attack, Attack },
    };

}
