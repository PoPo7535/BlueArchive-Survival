using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;


public class PlayerState : PlayerComponent
{
    public int hp = 100;
    public int maxHp = 100;
    public int attack;
    public int defensive;
    public int speed;
    public override void Init(PlayerBase player)
    {
        base.Init(player);
        player.state = this;
    }

    public void Damage(int damage)
    {
        hp -= damage;
    }

}
