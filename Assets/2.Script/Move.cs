using UnityEngine;

public class Move : MonoBehaviour
{
    private Rigidbody rigi;
    public float moveSpeed = 30;
    public float rotateSpeed = 30;
    public Animator ani;
    public Camera mainCamera;
    public GameObject bullet;
    private bool isShooting;
    
    private void Start()
    {
        rigi = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        var input = new Vector2(x, y);
        var dir = new Vector2(x, y).normalized * (moveSpeed * Time.deltaTime);

       
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
        if (Input.GetMouseButtonDown(0))
        {
            Shot();
            ani.SetTrigger(StringToHash.Attack);
            Instantiate(bullet, transform.position + (transform.forward + Vector3.up) * 0.5f, transform.rotation);
            return;
        }

        if (input is { x: 0, y: 0 })
            return;
        
        var targetRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.y));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }
    private void Shot()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        var plane = new Plane(Vector3.up, transform.position);
        if (false == plane.Raycast(ray, out var distance)) 
            return;
        
        var targetPoint = ray.GetPoint(distance);
        var dir = (targetPoint - transform.position).normalized;
        var lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime*10000);
    }
}
