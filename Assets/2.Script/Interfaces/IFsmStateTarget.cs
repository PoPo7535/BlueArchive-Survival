public interface IFsmStateTarget 
{
    void OnInit(IFsmStateOther other);
    void OnEnter();
    void OnExit();
    void OnUpdate(Spawner.NetworkInputData data);
    
}
