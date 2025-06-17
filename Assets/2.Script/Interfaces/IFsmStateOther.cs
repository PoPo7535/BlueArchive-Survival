public interface IFsmStateOther 
{
    void ChangeState(FsmState newState);
    void Move(Spawner.NetworkInputData data);
    void Rotation(Spawner.NetworkInputData data);
    T GetComponent<T>();
    bool CanAttack();
}