using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterBase))]
public class CharacterAbilities : MonoBehaviour
{
    [Tooltip("Can the player use ability 1?")]
    public bool ability1Toggle = true;
    [Tooltip("Can the player use ability 2?")]
    public bool ability2Toggle = true;

    [Tooltip("How long the cooldown is for ability 1.")]
    [SerializeField] private float ability1Cooldown = 3.0f;
    [Tooltip("How long the cooldown is for ability 2.")]
    [SerializeField] private float ability2Cooldown = 3.0f;

    [Tooltip("The UI slider for ability 1.")]
    [SerializeField] private Slider uiSlider1;
    [Tooltip("The UI slider for ability 2.")]
    [SerializeField] private Slider uiSlider2;

    [Tooltip("Should ability 1 be charged up, or on a cooldown?")]
    [SerializeField] private bool ability1Chargeup = false;
    [Tooltip("Should ability 2 be charged up, or on a cooldown?")]
    [SerializeField] private bool ability2Chargeup = false;

    [Tooltip("How fast the player will move whilst charging ability 1.")]
    [SerializeField] private float ability1ChargeSpeed;
    [Tooltip("How fast the player will move whilst charging ability 2.")]
    [SerializeField] private float ability2ChargeSpeed;

    [Header("Ability Callbacks")]
    [Tooltip("The callback which is invoked when ability 1 is pressed.")]
    [SerializeField] private UnityEvent<InputAction.CallbackContext> ability1Callback;
    [Tooltip("The callback which is invoked when ability 2 is pressed.")]
    [SerializeField] private UnityEvent<InputAction.CallbackContext> ability2Callback;

    [Tooltip("the aimer for the first ability(IF THE ABILITY DOESNT USE IT LEAVE THIS EMPTY)")]
    [SerializeField]
    private AimChecker ability1Aimer;
    [Tooltip("the aimer for the SECOND ability(IF THE ABILITY DOESNT USE IT LEAVE THIS EMPTY)")]
    [SerializeField]
    private AimChecker ability2Aimer;

    private float _ability1Timer = 0.0f;
    private float _ability2Timer = 0.0f;

    private CharacterBase _characterBase;

    void Start()
    {
        _characterBase = GetComponent<CharacterBase>();
    }

    void Update()
    {
        if (_ability1Timer > 0.0f)
            _ability1Timer -= Time.deltaTime;
        if (_ability2Timer > 0.0f)
            _ability2Timer -= Time.deltaTime;

        if (uiSlider1 != null)
        {
            uiSlider1.minValue = 0.0f;
            uiSlider1.maxValue = ability1Cooldown;
            uiSlider1.value = ability1Cooldown - _ability1Timer;
        }
        if (uiSlider2 != null)
        {
            uiSlider2.minValue = 0.0f;
            uiSlider2.maxValue = ability2Cooldown;
            uiSlider2.value = ability2Cooldown - _ability2Timer;
        }
    }

    public void DoAbility1(InputAction.CallbackContext context)
    {
        if (ability1Toggle == false)
            return;

        if (ability1Chargeup == false)
        {
            if (context.performed)
            {
                if (_ability1Timer <= 0.0f)
                {
                    if (ability1Callback != null)
                    {
                        if(ability1Aimer)
                        {
                            if(ability1Aimer._colliding)
                            {
                                return;
                            }
                        }
                        ability1Callback.Invoke(context);
                    }

                    _ability1Timer = ability1Cooldown;
                }
            }
        }
        else
        {
            if (context.performed)
            {
                _ability1Timer = ability1Cooldown;
                _characterBase.ChangeCurrentSpeed(ability1ChargeSpeed);
            }
            else if (context.canceled)
            {
                if (_ability1Timer <= 0.0f)
                {
                    if (ability1Callback != null)
                    {
                        if(ability1Aimer)
                        {
                            if(ability1Aimer._colliding)
                            {
                                return;
                            }
                        }
                    }
                    ability1Callback.Invoke(context);
                }
                _characterBase.ChangeCurrentSpeed(_characterBase.originalSpeed);
            }
        }
    }

    public void DoAbility2(InputAction.CallbackContext context)
    {
        if (ability2Toggle == false)
            return;

        if (ability2Chargeup == false)
        {
            if (context.performed)
            {
                if (_ability2Timer <= 0.0f)
                {
                    if (ability2Callback != null)
                    {
                        if (ability2Aimer)
                        {
                            if (ability2Aimer._colliding)
                            {
                                return;
                            }
                        }
                    }
                    ability2Callback.Invoke(context);

                    _ability2Timer = ability2Cooldown;
                }
            }
        }
        else
        {
            if (context.performed)
            {
                _ability2Timer = ability2Cooldown;
                _characterBase.ChangeCurrentSpeed(ability2ChargeSpeed);
            }
            else if (context.canceled)
            {
                if (_ability2Timer <= 0.0f)
                {
                    if (ability2Callback != null)
                    {
                        if (ability2Aimer)
                        {
                            if (ability2Aimer._colliding)
                            {
                                return;
                            }
                        }
                    }
                    ability2Callback.Invoke(context);
                }
                _characterBase.ChangeCurrentSpeed(_characterBase.originalSpeed);
            }
        }
    }
}
