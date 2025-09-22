using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class FsmMonsterMove : FsmMonsterBase
{
    private NetworkObject target;
    public FsmMonsterMove(IFsmStateOther other) : base(other) { }

    public override void OnEnter()
    {
        var player = App.I.GetAllPlayers();
        target = App.I.GetPlayerInfo(player[0]).PlayerObject.Object;
    }

    public override void OnUpdate(NetworkInputData _)
    {
        _controller.AniTrigger = StringToHash.Move;
        other.Move(target:target.transform);
        other.Rotation(target:target.transform);
    }
}
