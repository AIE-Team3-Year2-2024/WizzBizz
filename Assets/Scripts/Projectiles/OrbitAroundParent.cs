using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitAroundParent : MonoBehaviour
{
    [SerializeField] private float _distance = 5.0f;
    [SerializeField] private float _orbitSpeed = 1.0f;
    [SerializeField] private float _followSpeed = 1.0f;

    [SerializeField] private float _heightOffset;

    [SerializeField] [Range(0.0f, 1.0f)] private float _offset = 0.0f;

    private float _orbitAmount = 0.0f;
    private Vector3 _targetPosition = Vector3.zero;

    private float TAU = Mathf.PI * 2.0f;

    private Transform target;

    /// <summary>
    /// sets the target position from this objects parents position
    /// </summary>
    void Start()
    {
        target = transform.parent;

        _targetPosition.x = target.position.x;
        _targetPosition.y = target.position.y;
        _targetPosition.z = target.position.z;
    }

    /// <summary>
    /// update the position of this object based on the parents position and time
    /// </summary>
    void Update()
    {
        //increase orbit amount by time and speed
        _orbitAmount += Time.deltaTime * _orbitSpeed;
        //limit the orbit amount to be under one
        if (_orbitAmount >= 1.0f)
            _orbitAmount = 0.0f;

        //apply smoothing
        Vector3 targetPos = new Vector3(target.position.x, target.position.y, target.position.z);
        _targetPosition = Vector3.Slerp(_targetPosition, targetPos, Time.deltaTime * _followSpeed);

        //update the x and z position by sin and cos
        Vector3 orbitVector = Vector3.zero;
        orbitVector.x = _targetPosition.x + Mathf.Sin(_offset * TAU + _orbitAmount * TAU) * _distance;
        orbitVector.z = _targetPosition.z + Mathf.Cos(_offset * TAU + _orbitAmount * TAU) * _distance;
        transform.position = orbitVector + Vector3.up * _heightOffset;
    }
}
