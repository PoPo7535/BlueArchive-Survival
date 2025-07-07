using UnityEngine;

public interface IFsmStateOther 
{
    void ChangeState(FsmState newState);
    void Move(NetworkInputData data = default, Transform target = null);
    void Rotation(NetworkInputData data = default, Transform target = null);
    void OnAttack() { }
    T GetComponent<T>();
    bool CanAttack();
}