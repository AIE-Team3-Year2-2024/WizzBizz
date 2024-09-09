using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [Tooltip("the direction any object being hit by this will move")]
    [SerializeField]
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        other.transform.position += direction * Time.deltaTime;
    }
}
