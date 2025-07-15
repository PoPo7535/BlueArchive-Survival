using Fusion;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class PlayerComponent : NetworkBehaviour, IPlayerComponent
{
    [Read] public PlayerBase Player { get; private set; }

    public virtual void Init(PlayerBase player)
    {
        Player = player;
    }
}

