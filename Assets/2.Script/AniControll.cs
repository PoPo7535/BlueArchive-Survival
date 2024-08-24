using System;
using UnityEngine;

public class AniControll : MonoBehaviour
{
    public Animator ani;

    public void SetAni(int id, bool active) => ani.SetBool(id, active);
    public void SetAni(int id) => ani.SetTrigger(id);
    
    private void Start()
    {
    }

    private void Update()
    {
        
    }
}
