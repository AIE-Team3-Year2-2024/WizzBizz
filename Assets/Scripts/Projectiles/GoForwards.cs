using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoForwards : MonoBehaviour
{

    [Tooltip("the speed this object will move in its forwards direction")]
    [SerializeField]
    private float speed;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }
}
