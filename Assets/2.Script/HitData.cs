using Fusion;

public struct HitData : INetworkStruct
{
    public PlayerRef other;
    public float damage;
}