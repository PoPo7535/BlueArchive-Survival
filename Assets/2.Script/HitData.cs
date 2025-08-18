using Fusion;

public struct HitData : INetworkStruct
{
    public PlayerRef other;
    public int damage;
}