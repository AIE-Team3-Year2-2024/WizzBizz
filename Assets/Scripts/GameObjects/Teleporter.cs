using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    private Transform[] endPoints;
    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = endPoints[Random.Range(0, endPoints.Length)].position;
    }
}
