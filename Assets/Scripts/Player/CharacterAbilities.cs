using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterAbilities : MonoBehaviour
{
    [Tooltip("How long the cooldown is for ability 1.")]
    [SerializeField] private float ability1Cooldown = 3.0f;
    [Tooltip("How long the cooldown is for ability 2.")]
    [SerializeField] private float ability2Cooldown = 3.0f;

    [Header("Ability Callbacks")]
    [Tooltip("The callback which is invoked when ability 1 is pressed.")]
    [SerializeField] private UnityEvent<InputAction.CallbackContext> ability1Callback;
    [Tooltip("The callback which is invoked when ability 2 is pressed.")]
    [SerializeField] private UnityEvent<InputAction.CallbackContext> ability2Callback;

    private float _ability1Timer = 0.0f;
    private float _ability2Timer = 0.0f;

    void Update()
    {
        if (_ability1Timer > 0.0f)
            _ability1Timer -= Time.deltaTime;
        if (_ability2Timer > 0.0f)
            _ability2Timer -= Time.deltaTime;
    }

    public void DoAbility1(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (_ability1Timer <= 0.0f)
            {
                if (ability1Callback != null)
                    ability1Callback.Invoke(context);

                _ability1Timer = ability1Cooldown;
            }
        }
    }

    public void DoAbility2(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (_ability2Timer <= 0.0f)
            {
                if (ability2Callback != null)
                    ability2Callback.Invoke(context);

                _ability2Timer = ability2Cooldown;
            }
        }
    }
}
