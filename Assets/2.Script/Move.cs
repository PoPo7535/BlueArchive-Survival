using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

public class Move : MonoBehaviour
{
    private Rigidbody rigi;
    public float moveSpeed = 30;
    public float rotateSpeed = 30;
    public Animator ani;
    public Camera mainCamera;
    public Transform weaponTr;
    public GameObject bullet;
    
    private void Start()
    {
        rigi = GetComponent<Rigidbody>();
    }

    void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        if (Input.GetMouseButton(0))
        {
            Foo();
            ani.SetTrigger(StringToHash.Attack);
        }
        if (Input.GetMouseButtonUp(0))
        {
            Instantiate(bullet, transform.transform.position + ((transform.forward + Vector3.up) * 0.5f),
                transform.rotation);
        }
        if (x == 0 && y == 0)
        {
            rigi.velocity = new Vector3(0, rigi.velocity.y, 0);
            ani.SetTrigger(StringToHash.Idle);
            return; 
        }


        ani.SetTrigger(StringToHash.Move);
        var dir = new Vector2(x, y).normalized * (moveSpeed * Time.deltaTime);
        rigi.velocity = new Vector3(dir.x, rigi.velocity.y, dir.y);
        var targetRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.y));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    void Foo()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        var plane = new Plane(Vector3.up, transform.position);
        if (false == plane.Raycast(ray, out var distance)) 
            return;
        
        var targetPoint = ray.GetPoint(distance);
        var dir = (targetPoint - transform.position).normalized;
        var lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
    }
}
