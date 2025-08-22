using Fusion;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public float speed = 100f;
    [Networked] private TickTimer timer { get; set; }
    private bool _spawn = false;

    public override void Spawned()
    {
        timer = TickTimer.CreateFromSeconds(Runner, 3f);
        _spawn = true;
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        timer = TickTimer.None;
        _spawn = false;
    }
    
    public override void FixedUpdateNetwork()
    {
        if (false == HasStateAuthority)
            return;

        if (timer.ExpiredOrNotRunning(Runner))
        {
            Runner.Despawn(Object);
            return;
        }
        
        transform.Translate(Vector3.forward * (speed * Runner.DeltaTime),Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (false == _spawn)
            return;
        
        if (other.CompareTag($"Monster"))
        {
            other.GetComponent<MonsterHitManager>().Damage(Object.InputAuthority, 10f);
            Runner.Despawn(Object);
        }
    }
}
