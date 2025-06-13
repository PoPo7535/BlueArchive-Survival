using Fusion;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class PlayerComponent : NetworkBehaviour, IPlayerComponent
{
    [Read] public PlayerBase PB => playerBase;
    private PlayerBase playerBase;

    public virtual void Init(PlayerBase player)
    {
        playerBase = player;
    }
}

