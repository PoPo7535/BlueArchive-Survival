public interface IFsmStateTarget 
{
    void OnEnter();
    void OnExit();
    void OnUpdate(NetworkInputData data);
}
