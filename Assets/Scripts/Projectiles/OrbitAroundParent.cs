using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitAroundParent : MonoBehaviour
{
    [SerializeField] private float _distance = 5.0f;
    [SerializeField] private float _orbitSpeed = 1.0f;
    [SerializeField] private float _followSpeed = 1.0f;

    [SerializeField] [Range(0.0f, 1.0f)] private float _offset = 0.0f;

    private float _orbitAmount = 0.0f;
    private Vector3 _targetPosition = Vector3.zero;

    private float TAU = Mathf.PI * 2.0f;

    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = transform.parent;

        _targetPosition.x = target.position.x;
        _targetPosition.z = target.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        _orbitAmount += Time.deltaTime * _orbitSpeed;
        if (_orbitAmount >= 1.0f)
            _orbitAmount = 0.0f;

        Vector3 targetPos = new Vector3(target.position.x, 0.0f, target.position.z);
        _targetPosition = Vector3.Slerp(_targetPosition, targetPos, Time.deltaTime * _followSpeed);

        Vector3 orbitVector = Vector3.zero;
        orbitVector.x = _targetPosition.x + Mathf.Sin(_offset * TAU + _orbitAmount * TAU) * _distance;
        orbitVector.z = _targetPosition.z + Mathf.Cos(_offset * TAU + _orbitAmount * TAU) * _distance;
        transform.position = orbitVector;
    }
}
