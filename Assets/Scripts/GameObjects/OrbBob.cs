using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbBob : MonoBehaviour
{
    public bool shouldBob = true;

    [SerializeField] private float _floatSpeed = 1.0f;
    [SerializeField] private float _floatDistance = 1.0f;

    private float _floatTimer = 0.0f;

    void Update()
    {
        if (!shouldBob)
            return;

        _floatTimer += Time.deltaTime;
        float halfDistance = (_floatDistance / 2.0f);
        transform.localPosition = (Vector3.up * halfDistance) + (Vector3.up * Mathf.Sin(_floatSpeed * _floatTimer) * halfDistance);
        if (_floatTimer > (Mathf.PI * 2.0f) / _floatSpeed)
            _floatTimer = 0.0f;
    }
}
