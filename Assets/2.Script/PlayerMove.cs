using Fusion;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : PlayerComponent
{
    [ShowInInspector] private Rigidbody rigi;
    [Fusion.ReadOnly] public Animator ani;
    [Fusion.ReadOnly] public Camera mainCamera;
    public GameObject bullet;
    public float moveSpeed = 30;
    public float rotateSpeed = 30;

    private float attackDelay = 0;
    private const float attackDelayMax = 2f;

    public override void Init(PlayerBase player)
    {
        base.Init(player);
        PB = player;
        PB.playerMove = this;
    }
    private void Awake()
    {
        rigi = GetComponent<Rigidbody>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out Spawner.NetworkInputData data))
        {
            var dir = data.input.normalized * (moveSpeed * Runner.DeltaTime);
            Move(data.input, dir);
            Rotation(data.input, dir);
        }
    }

    private void Move(Vector2 input, Vector2 dir)
    {
        if (input is { x: 0, y: 0 })
        {
            rigi.velocity = new Vector3(0, rigi.velocity.y, 0); 
            return; 
        }
        rigi.velocity = new Vector3(dir.x, rigi.velocity.y, dir.y);
    }

    private void Rotation(Vector2 input, Vector2 dir)
    {
        attackDelay += Runner.DeltaTime;

        if (false == PB.findEnemy.nearObj.IsUnityNull() && attackDelayMax < attackDelay) 
        {
            Shot();
            attackDelay = 0;
            Runner.Spawn(bullet, transform.position + (transform.forward + Vector3.up) * 0.5f, transform.rotation);
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
        
        var targetPoint = PB.findEnemy.nearObj.transform.position;
        var dir = (targetPoint - transform.position).normalized;
        var lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10000);
    }


}
