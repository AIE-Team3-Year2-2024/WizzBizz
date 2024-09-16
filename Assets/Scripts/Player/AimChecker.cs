using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimChecker : MonoBehaviour
{

    [HideInInspector]
    public bool _colliding;

    private MeshRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerStay(Collider other)
    {
        _colliding = true;
        _renderer.material.color = Color.red;
    }

    private void FixedUpdate()
    {
        _colliding = false;
        _renderer.material.color = Color.green;
    }


}
