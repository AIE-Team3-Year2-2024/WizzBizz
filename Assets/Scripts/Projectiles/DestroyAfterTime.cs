using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [Tooltip("how long this object will live for")]
    [SerializeField]
    private float _time;
    
    /// <summary>
    /// calls destroy on this object with the inputted time
    /// </summary>
    void Start()
    {
        Destroy(gameObject, _time);
    }
}
