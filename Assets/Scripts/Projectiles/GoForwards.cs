using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoForwards : MonoBehaviour
{

    [Tooltip("the speed this object will move in its forwards direction")]
    [SerializeField]
    private float speed;

    /// <summary>
    /// moves this obejct in its forwards direction by its speed and delta time
    /// </summary>
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }
}
