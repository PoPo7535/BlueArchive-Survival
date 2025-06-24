using System.Linq;
using Fusion;
using UnityEngine;

public class Monster : NetworkBehaviour
{
    public Rigidbody rigi;
    public float moveSpeed = 300f;
    public float rotateSpeed = 10f;
    public Animator ani;
    public GameObject expObj;

    public NetworkObject target;

    public override void Spawned()
    {
        base.Spawned();
        var player = App.I.GetAllPlayers();
        target = App.I.GetPlayerInfo(player[0]).Obj;

    }

    public override void FixedUpdateNetwork()
    {
        ani.SetTrigger(StringToHash.Move);
        if (false == HasStateAuthority)
            return;
        var dir = (target.transform.position - transform.position).normalized *
                  (moveSpeed * Runner.DeltaTime);
        rigi.velocity = new Vector3(dir.x, rigi.velocity.y, dir.z);
        var targetRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Runner.DeltaTime);
    }

    public void Damage(PlayerRef other = default)
    {
        Instantiate(expObj, transform.position, Quaternion.identity);
        Runner.Despawn(Object);
    }
}
