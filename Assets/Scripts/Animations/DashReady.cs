using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashReady : MonoBehaviour
{
    void Start()
    {
        Animator animator = GetComponentInChildren<Animator>();
        
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }
}
