using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimChecker : MonoBehaviour
{
    [HideInInspector]
    public int currentCollisions;

    private MeshRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        currentCollisions++;
        renderer.material.color = Color.red;
    }

    private void OnTriggerExit(Collider other)
    {
        currentCollisions--;
        if(currentCollisions < 1)
        {
            renderer.material.color = Color.green;
        }
    }
}
