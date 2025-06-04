using Fusion;
public class PlayerComponent : NetworkBehaviour, IPlayerComponent
{
    public PlayerBase PB;
    public virtual void Init(PlayerBase player) { }
}

interface IPlayerComponent
{
    public void Init(PlayerBase player);
}