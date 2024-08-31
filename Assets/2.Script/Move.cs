using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class Move : NetworkBehaviour
{
    private Rigidbody rigi;
    private FindEnemy findEnemy;
    public float moveSpeed = 30;
    public float rotateSpeed = 30;
    public NetworkMecanimAnimator ani;
    public Camera mainCamera;
    public GameObject bullet;

    private float attackDelay = 0;
    private const float attackDelayMax = 2f;
    private void Awake()
    {
        rigi = GetComponent<Rigidbody>();
        findEnemy = GetComponent<FindEnemy>();
    }


    public override void FixedUpdateNetwork()
    {
        if (false == HasInputAuthority)
            return;
        
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        var input = new Vector2(x, y);
        var dir = new Vector2(x, y).normalized * (moveSpeed * Runner.DeltaTime);

        Move2(input, dir);
        Rotation(input, dir);
    }

    private void Move2(Vector2 input, Vector2 dir)
    {
        if (input is { x: 0, y: 0 })
        {
            rigi.velocity = new Vector3(0, rigi.velocity.y, 0);
            ani.SetTrigger(StringToHash.Idle);
            return; 
        }
        ani.SetTrigger(StringToHash.Move);
        rigi.velocity = new Vector3(dir.x, rigi.velocity.y, dir.y);
    }

    private void Rotation(Vector2 input, Vector2 dir)
    {

        attackDelay += Runner.DeltaTime;
        if (ani.Animator.GetCurrentAnimatorStateInfo(0).shortNameHash == StringToHash.Attack)
            return;
        
        if (false == findEnemy.nearObj.IsUnityNull() &&attackDelayMax < attackDelay) 
        {
            Shot();
            attackDelay = 0;
            ani.SetTrigger(StringToHash.Attack);
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
