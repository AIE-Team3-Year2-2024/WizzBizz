using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Silence : MonoBehaviour
{
    [HideInInspector]
    public float lifeTime;

    private PlayerInput _input;

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            enabled = false;
        }
    }

    private void OnEnable()
    {
        if (_input == null)
        {
            _input = GetComponent<PlayerInput>();
        }

        _input.actions.FindAction("Ability 1").Disable();
        _input.actions.FindAction("Ability 2").Disable();
    }

    private void OnDisable()
    {
        _input.actions.FindAction("Ability 1").Enable();
        _input.actions.FindAction("Ability 2").Enable();
    }
}
