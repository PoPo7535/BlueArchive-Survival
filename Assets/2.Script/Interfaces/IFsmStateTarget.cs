public interface IFsmStateTarget 
{
    void OnEnter();
    void OnExit();
    void OnUpdate(Spawner.NetworkInputData data);
}
