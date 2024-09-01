using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class Move : NetworkBehaviour, ISpawned
{
    private Rigidbody rigi;
    private FindEnemy findEnemy;
    public Animator ani;
    public Camera mainCamera;
    public GameObject bullet;
    [Networked] private int aniTrigger { get; set; }
    public float moveSpeed = 30;
    public float rotateSpeed = 30;

    private float attackDelay = 0;
    private const float attackDelayMax = 2f;
    private void Awake()
    {
        rigi = GetComponent<Rigidbody>();
        findEnemy = GetComponent<FindEnemy>();
    }

    public override void Spawned()
    {
        if (HasInputAuthority)
            GameManager.I.player = gameObject;
    }

    public override void FixedUpdateNetwork()
    {
        // if (false == HasStateAuthority)
        //     return;
        ani.SetTrigger(aniTrigger);
        if (GetInput(out Spawner.NetworkInputData data))
        {
            var dir = data.input.normalized * (moveSpeed * Runner.DeltaTime);
            Move2(data.input, dir);
            Rotation(data.input, dir);
        }
    }

    private void Move2(Vector2 input, Vector2 dir)
    {
        if (input is { x: 0, y: 0 })
        {
            rigi.velocity = new Vector3(0, rigi.velocity.y, 0);
            aniTrigger = StringToHash.Idle;
            return; 
        }
        aniTrigger = StringToHash.Move;
        rigi.velocity = new Vector3(dir.x, rigi.velocity.y, dir.y);
    }

    private void Rotation(Vector2 input, Vector2 dir)
    {

        attackDelay += Runner.DeltaTime;
        if (ani.GetCurrentAnimatorStateInfo(0).shortNameHash == StringToHash.Attack)
            return;
        
        if (false == findEnemy.nearObj.IsUnityNull() &&attackDelayMax < attackDelay) 
        {
            Shot();
            attackDelay = 0;
            aniTrigger = StringToHash.Attack;
            Instantiate(bullet, transform.position + (transform.forward + Vector3.up) * 0.5f, transform.rotation);
            return;
        }

        if (input is { x: 0, y: 0 })
            return;
        
        var targetRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.y));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Runner.DeltaTime);
    }
    private void Shot()
    {
        // var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        // var plane = new Plane(Vector3.up, transform.position);
        // if (false == plane.Raycast(ray, out var distance)) 
        //     return;
        // var targetPoint = ray.GetPoint(distance);

        var targetPoint = findEnemy.nearObj.transform.position;
        var dir = (targetPoint - transform.position).normalized;
        var lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10000);
    }
}
