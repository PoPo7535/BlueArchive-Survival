using UnityEngine;

public class Monster : MonoBehaviour
{
    public Rigidbody rigi;
    public float moveSpeed = 300f;
    public float rotateSpeed = 10f;
    public Animator ani;
        
    void Update()
    {
        var dir = (GameManager.I.player.transform.position - transform.position).normalized *
                  (moveSpeed * Time.deltaTime);
        rigi.velocity = new Vector3(dir.x, rigi.velocity.y, dir.z);
        var targetRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        ani.SetTrigger(StringToHash.Move);
    }

    public void Damage()
    {
        gameObject.SetActive(false);
    }
}
