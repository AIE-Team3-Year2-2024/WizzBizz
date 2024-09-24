using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Silence : MonoBehaviour
{
    [HideInInspector]
    public float lifeTime;

    private PlayerInput _input;

    /// <summary>
    /// manage lifetime and whther it is enabled
    /// </summary>
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            enabled = false;
        }
    }

    /// <summary>
    /// apply effect to player
    /// </summary>
    private void OnEnable()
    {
        if (_input == null)
        {
            _input = GetComponent<PlayerInput>();
        }

        _input.actions.FindAction("Ability 1").Disable();
        _input.actions.FindAction("Ability 2").Disable();
    }

    /// <summary>
    /// set the player back to normal
    /// </summary>
    private void OnDisable()
    {
        _input.actions.FindAction("Ability 1").Enable();
        _input.actions.FindAction("Ability 2").Enable();
    }
}
