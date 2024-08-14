using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class SnakeProjectileMovement : MonoBehaviour
{
    [Tooltip("the multyplier for how far left and right this projectile will 'snake'")]
    [SerializeField]
    private float _distanceMult;

    [Tooltip("speed")]
    [SerializeField]
    private float _speed;

    private float _progress = 0.25f;

    // Update is called once per frame
    void Update()
    {
        float wiggle = Mathf.Sin(_progress * (2 * Mathf.PI));
        transform.position += transform.right.normalized * wiggle * _distanceMult * _speed;
        _progress += Time.deltaTime * _speed;

        /*if (_progress > 1.0f)
            _progress = 0.0f;*/
    }
}
