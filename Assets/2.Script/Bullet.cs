using Fusion;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public float speed = 100f;
    [Networked] private TickTimer timer { get; set; }

    public override void Spawned()
    {
        timer = TickTimer.CreateFromSeconds(Runner, 3f);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        timer = TickTimer.None;
    }
    
    public override void FixedUpdateNetwork()
    {
        if (false == HasStateAuthority)
            return;
        
        if (timer.ExpiredOrNotRunning(Runner))
            Runner.Despawn(Object);
        
        transform.Translate(Vector3.forward * (speed * Time.deltaTime),Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag($"Enemy"))
        {
            other.GetComponent<Monster>().Damage();
            Runner.Despawn(Object);
        }
    }
}
