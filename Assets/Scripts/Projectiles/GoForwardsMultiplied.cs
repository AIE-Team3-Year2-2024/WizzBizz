using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoForwardsMultiplied : MonoBehaviour
{
    [Tooltip("the speed this object will move in its forwards direction")]
    [SerializeField]
    private float _speed;

    [Tooltip("what the speed will be multiplied by")]
    [SerializeField]
    private float _speedMult;

    private float _currentSpeed;

    private void Start()
    {
        _currentSpeed = _speed;
    }

    /// <summary>
    /// moves this obejct in its forwards direction by its speed and delta time
    /// </summary>
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * _currentSpeed;

        _currentSpeed += _speedMult * Time.deltaTime;
    }
}
