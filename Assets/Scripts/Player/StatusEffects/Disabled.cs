using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Disabled : MonoBehaviour
{
    [HideInInspector]
    public float lifeTime;

    private PlayerInput _input;

    private int _currentAbility;

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

        _currentAbility = Random.Range(1, 3);

        _input.actions.FindAction("Ability " + _currentAbility).Disable();
    }

    /// <summary>
    /// set the player back to normal
    /// </summary>
    private void OnDisable()
    {
        _input.actions.FindAction("Ability " + _currentAbility).Enable();
    }
}
