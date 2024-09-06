using System;
using Fusion;

public class PlayerInfo : NetworkBehaviour
{

    [Networked, OnChangedRender(nameof(Test))] public NetworkString<_16> PlayerName { get; set; }
    [Networked] public int CharIndex { get; set; }


    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Test()
    {
        gameObject.name = $"{PlayerName} Info";

    }
    public override void Spawned()
    {
        Runner.SetPlayerObject(Object.InputAuthority, Object);
        PlayerName = "123";
    }
}
