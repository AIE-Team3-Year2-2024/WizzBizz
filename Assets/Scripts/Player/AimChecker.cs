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

    /// <summary>
    /// set colliding to false 
    /// </summary>
    private void FixedUpdate()
    {
        _colliding = false;
        _renderer.material.color = Color.green;
    }

    /// <summary>
    /// if this is hitting something set colliding to true
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        _colliding = true;
        _renderer.material.color = Color.red;
    }

    


}
