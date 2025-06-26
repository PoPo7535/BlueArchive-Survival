using UnityEngine;

public interface IFsmStateOther 
{
    void ChangeState(FsmState newState);
    void Move(Spawner.NetworkInputData data = default, Transform target = null);
    void Rotation(Spawner.NetworkInputData data = default, Transform target = null);
    void OnAttack() { }
    T GetComponent<T>();
    bool CanAttack();
}