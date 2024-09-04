using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimChecker : MonoBehaviour
{

    [HideInInspector]
    public bool _colliding;

    private MeshRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerStay(Collider other)
    {
        _colliding = true;
        renderer.material.color = Color.red;
    }

    private void FixedUpdate()
    {
        _colliding = false;
        renderer.material.color = Color.green;
    }


}
