using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;


[RequireComponent(typeof(Weakness))]
[RequireComponent(typeof(Crippled))]
[RequireComponent(typeof(Stun))]
[RequireComponent(typeof(Confusion))]
[RequireComponent(typeof(Disabled))]
[RequireComponent(typeof(Silence))]
[RequireComponent(typeof(Vitality))]
[RequireComponent(typeof(Slow))]
[RequireComponent(typeof(Haste))]
[RequireComponent(typeof(Burn))]
[RequireComponent(typeof(Poison))]
[RequireComponent(typeof(Cure))]
[RequireComponent(typeof(Dementia))]
public class CharacterBase : MonoBehaviour
{
    [HideInInspector] public bool hasOrb = false;
    [HideInInspector] public GameObject heldOrb = null;

    [HideInInspector] public Gamepad playerGamepad = null;

    private PlayerInput input;

    [Tooltip("The maximum speed this character will move at.")]
    [SerializeField]
    private float _speed;

    [HideInInspector]
    public float originalSpeed;

    //[SerializeField]
    //[Tooltip("The rate at which the character will accelerate towards the max speed.")]
    private float _acceleration;
    [Tooltip("The amount the character will decelerate when it is not moving. This is a percentage (0-1).")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float _deceleration;

    private float _originalAccel;
    private float _originalDecel;
    private float _origanalHealth;

    private bool _shouldStopSliding = false;

    [Tooltip("this is the trigger collider used for catching")]
    public Collider catchTrigger;

    [Tooltip("how long this character will dash for")]
    [SerializeField]
    private float _dashTime;

    [Tooltip("the speed this charcater will dash at")]
    [SerializeField]
    private float _dashSpeed;

    [Tooltip("how long the player must wait to dash agian AFTER the dash has completed")]
    [SerializeField]
    private float _dashWaitTime;

    [Tooltip("the speed the player will move at if chgarging ann attack")]
    [SerializeField]
    private float _chargeSpeed;

    [Tooltip("how long the catch trigger will be active")]
    [SerializeField]
    private float catchParryTime;

    [Tooltip("how long after the catch trigger activates where the player cant move and is vulnrable")]
    [SerializeField]
    private float catchWaitTime;

    [Tooltip("the players health")]
    [SerializeField]
    private float _health;

    [Tooltip("how long the player will have the orb before it dissapears")]
    [SerializeField]
    private float _ballLifetime;

    [Tooltip("whether or not on move will be skipped")]
    [HideInInspector]
    public bool canMove = true;

    [Tooltip("whether or not on dash will be skipped")]
    [HideInInspector]
    public bool canDash = true;


    [Tooltip("this value multiplies the size of the pointer aimer when aiming")]
    [SerializeField]
    private float _pointerAimerRange;

    [HideInInspector]
    public float currentAimMagnitude;

    [Tooltip("the amount of time needed to charge the basic attack")]
    [SerializeField]
    private float _basicAttackTime;

    //[HideInInspector]
    public float damageMult = 1;

    private float _basicAttackTimer = 0;

    [Tooltip("the image used to show where the player is aiming")]
    [SerializeField]
    private RectTransform _pointerAimer;

    [Tooltip("the slider component of this players health bar")]
    [SerializeField]
    private Slider healthBar;

    [Tooltip("The slider component of the attack charge up bar.")]
    [SerializeField]
    private Slider attackChargeBar;

    [Tooltip("the Text on the player showing what number they are")]
    public TMP_Text playerNumber;

    private Vector3 _movementDirection;

    [Tooltip("where to spawn projectiles on this character")]
    public Transform _projectileSpawnPosition;

    [Header("Effects")]

    [Tooltip("weakness")]
    [SerializeField]
    private Weakness _weakness;

    [Tooltip("Crippled")]
    [SerializeField]
    private Crippled _crippled;

    [Tooltip("Stun")]
    [SerializeField]
    private Stun _stun;

    [Tooltip("Confusion")]
    [SerializeField]
    private Confusion _confusion;

    [HideInInspector]
    public bool confused;

    [Tooltip("Disabled")]
    [SerializeField]
    private Disabled _disabled;

    [Tooltip("Silence")]
    [SerializeField]
    private Silence _silence;

    [Tooltip("Vitality")]
    [SerializeField]
    private Vitality _vitality;

    [Tooltip("Slow")]
    [SerializeField]
    private Slow _slow;

    [Tooltip("Haste")]
    [SerializeField]
    private Haste _haste;

    [Tooltip("Burn")]
    [SerializeField]
    private Burn _burn;

    [Tooltip("Poison")]
    [SerializeField]
    private Poison _poison;

    [Tooltip("Cure")]
    [SerializeField]
    private Cure _cure;

    [Tooltip("Dementia")]
    [SerializeField]
    private Dementia _dementia;

    [Header("Trigger Attacks")]
    public UnityEvent ballAttack;
    public UnityEvent normalAttack;

    private Rigidbody rb;

    public enum StatusEffects
    {
        NONE, 
        CONFUSION,
        DISABLED,
        SILENCE,
        CRIPPLED,
        STUN,
        WEAKNESS,
        VITALITY,
        SLOW,
        HASTE,
        BURNING,
        POISON,
        CURE,
        DEMENTIA
    }

    private void Start()
    {
        input = GetComponent<PlayerInput>();
        catchTrigger.isTrigger = true;
        catchTrigger.enabled = false;

        originalSpeed = _speed;
        _originalAccel = _acceleration;
        _originalDecel = _deceleration;
        _origanalHealth = _health;

        if (healthBar)
        {
            healthBar.minValue = 0;
            healthBar.maxValue = _origanalHealth;
        }

        rb = GetComponent<Rigidbody>();

        damageMult = 1;
    }

    void Update()
    {
        _basicAttackTimer += Time.deltaTime;

        if (attackChargeBar != null)
        {
            attackChargeBar.maxValue = _basicAttackTime;
            attackChargeBar.value = _basicAttackTimer;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _acceleration = _speed * _deceleration;
        if (_movementDirection.magnitude > 0.0f)
            rb.AddForce(_movementDirection * _acceleration, ForceMode.VelocityChange);
        //_velocity += _movementDirection * _acceleration; // Add acceleration when there is input.

        //if (rb.velocity.magnitude > 0.0f && _movementDirection.magnitude <= 0.0f) // Only start decelerating when the character is moving, but also when there's no input.
            rb.velocity *= (1.0f - _deceleration);
        //_velocity *= (1.0f - _deceleration); // Invert the value so it's more intuitive in the inspector, so 0 is no deceleration instead of 1.

        //rb.velocity *= (_speed / (_speed + _acceleration)) * _deceleration;
        //rb.velocity = Vector3.ClampMagnitude(rb.velocity, _speed);

        //_velocity = Vector3.ClampMagnitude(_velocity, _speed); // Clamp the velocity to the maximum speed.
        //rb.position += _velocity * Time.fixedDeltaTime; // Apply the velocity to the character position.
        //rb.velocity = _velocity;
    }

    void LateUpdate()
    {
        if (rb.velocity.magnitude <= 0.0f || _movementDirection.magnitude > 0.0f) // If the character has stopped moving, or if there is input.
        {
            if (_shouldStopSliding) // The player should stop sliding now.
            {
                //_acceleration = _originalAccel; // Reset.
                _deceleration = _originalDecel;
                _shouldStopSliding = false;
            }
        }
    }

    /// <summary>
    /// this function is called by the players input controller wich will change the vector the charcater moves by
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            _movementDirection.z = context.ReadValue<Vector2>().y;
            _movementDirection.x = context.ReadValue<Vector2>().x;
        }
        if (confused)
        {
            _movementDirection = -_movementDirection;
        }
    }

    public void CancelPlayerMovement()
    {
        _movementDirection = Vector3.zero;
    }

    public void AddSpeed(float addition)
    {
        _speed += addition;
        originalSpeed += addition;
    }

    public void ChangeCurrentSpeed(float newSpeed)
    {
        _speed = newSpeed;
    }

    /// <summary>
    /// this function should be called by all charcters and will change the direction that this player faces
    /// </summary>
    /// <param name="context"></param>
    public void OnAim(InputAction.CallbackContext context)
    {
        Vector3 aimDirection = new Vector3();
        aimDirection.z = context.ReadValue<Vector2>().y;
        aimDirection.x = context.ReadValue<Vector2>().x;

        if(confused)
        {
            aimDirection = -aimDirection;
        }
                
        transform.LookAt(aimDirection += transform.position, transform.up);
        

        currentAimMagnitude = context.ReadValue<Vector2>().magnitude;

        _pointerAimer.localScale = new Vector3(_pointerAimer.localScale.x, 1 + (_pointerAimerRange * currentAimMagnitude), _pointerAimer.localScale.z);
    }

    /// <summary>
    /// should be called by all charcaters and calls DashRoutine()
    /// </summary>
    /// <param name="context"></param>
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            StartCoroutine(DashRoutine());
        }
    }

    /// <summary>
    /// takes the players ability to move and changes there speed and sets it back after _dashTime
    /// </summary>
    /// <returns></returns>
    public IEnumerator DashRoutine()
    {
        Vector3 oldMoveDir = _movementDirection;
        canMove = false;
        _speed = _dashSpeed;
        _movementDirection = _movementDirection.normalized;
        yield return new WaitForSeconds(_dashTime);
        canMove = true;
        _speed = originalSpeed;
        _movementDirection = oldMoveDir;
        StartCoroutine(WaitToDash());
    }

    public IEnumerator WaitToDash()
    {
        yield return new WaitForSeconds(_dashWaitTime);
        canDash = true;
    }

    public void OnCatch(InputAction.CallbackContext context)
    {
        StartCoroutine(CatchRoutine());
    }

    public IEnumerator CatchRoutine()
    {
        input.DeactivateInput();
        catchTrigger.enabled = true;
        yield return new WaitForSeconds(catchParryTime);
        catchTrigger.enabled = false;
        yield return new WaitForSeconds(catchWaitTime);
        input.ActivateInput();
    }

    public void StartSliding(float slipperyness, float accelFactor)
    {
        _shouldStopSliding = false;
        float inv = (1.0f - slipperyness);
        //_acceleration = _originalAccel * (slipperyness * accelFactor);
        _deceleration = inv;
    }

    public void StopSliding()
    {
        _shouldStopSliding = true;
    }

    public void TakeDamage(float damage)
    {
        if(_health <= 0)
        {
            return;
        }
        _health -= damage;

        if (healthBar)
        {
            healthBar.value = _health;
        }

        if (_health <= 0)
        {
            Death();
        }
    }

    public void TakeDamage(float damage, StatusEffects effect, float time)
    {
        if (_health <= 0)
        {
            return;
        }
        _health -= damage;

        if (healthBar)
        {
            healthBar.value = _health;
        }

        if (_health <= 0)
        {
            Death();
        }

        switch(effect)
        {
            case(StatusEffects.NONE):
                {
                    break;
                }

            case (StatusEffects.CONFUSION):
                {
                    if(_confusion.enabled == false)
                    {
                        _confusion.enabled = true;
                        _confusion.lifeTime = time;
                    }
                    break;
                }

            case (StatusEffects.DISABLED):
                {
                    if(_silence.enabled)
                    {
                        _silence.enabled = false;
                    }
                    if(_disabled.enabled == false)
                    {
                        _disabled.enabled = true;
                        _disabled.lifeTime = time;
                    }
                    break;
                }

            case (StatusEffects.SILENCE):
                {
                    if(_disabled.enabled)
                    {
                        _disabled.enabled = false;
                    }
                    if(_silence.enabled == false)
                    {
                        _silence.enabled = true;
                        _silence.lifeTime = time;
                    }
                    break;
                }

            case (StatusEffects.CRIPPLED):
                {
                    if(_crippled.enabled == false)
                    {
                        _crippled.enabled = true;
                        _crippled.lifeTime = time;
                    }
                    break;
                }

            case (StatusEffects.STUN):
                {
                    if(_stun.enabled == false)
                    {
                        _stun.enabled = true;
                        _stun.lifeTime = time;
                    }
                    break;
                }

            case (StatusEffects.WEAKNESS):
                {
                    if (_weakness.enabled == false)
                    {
                        _weakness.enabled = true;
                        _weakness.lifeTime = time;
                    }
                    break;
                }

            case (StatusEffects.VITALITY):
                {
                    if(_vitality.enabled == false)
                    {
                        _vitality.enabled = true;
                        _vitality.lifeTime = time;
                    }
                    break;
                }

            case (StatusEffects.SLOW):
                {
                    if(_slow.enabled == false)
                    {
                        _slow.enabled = true;
                        _slow.lifeTime = time;
                    }
                    break;
                }

            case (StatusEffects.HASTE):
                {
                    if (_haste.enabled == false)
                    {
                        _haste.enabled = true;
                        _haste.lifeTime = time;
                    }
                    break;
                }

            case (StatusEffects.BURNING):
                {
                    if(_burn.enabled == false)
                    {
                        _burn.enabled = true;
                        _burn.lifeTime = time;
                    }
                    break;
                }

            case (StatusEffects.POISON):
                {
                    if (_poison.enabled == false)
                    {
                        _poison.enabled = true;
                        _poison.lifeTime = time;
                    }
                    break;
                }

            case (StatusEffects.CURE):
                {
                    if(_cure.enabled == false)
                    {
                        _cure.enabled = true;
                        _cure.lifeTime = time;
                    }
                    break;
                }

            case (StatusEffects.DEMENTIA):
                {
                    if(_dementia.enabled == false)
                    {
                        _dementia.enabled = true;
                        _dementia.lifeTime = time;
                    }
                    break;
                }

        }
    }

    public void TakeKnockback(float amount, Vector3 dir)
    {
        if (dir != Vector3.zero && amount > 0.0f)
        {
            rb.AddForce(-dir * amount, ForceMode.Impulse);
        }
    }

    public void Death()
    {
        Debug.Log("Player has died: " + gameObject.name);

        canMove = false;
        _movementDirection = Vector3.zero;
        GameManager.Instance.PlayerDeath(this);
    }

    public virtual void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _basicAttackTimer = 0;
            if (hasOrb)
            {
                StartCoroutine(DoBallAttackHaptics());
                StopCoroutine(KillBall(null));
                ballAttack.Invoke();
                Destroy(heldOrb);
                heldOrb = null;
                hasOrb = false;
            }
            else
            {
                if (attackChargeBar != null)
                {
                    attackChargeBar.gameObject.SetActive(true);
                }

                Debug.Log("Start Attack Timer: " + _basicAttackTimer);

                _speed = _chargeSpeed;
            }
        }
        else if (context.canceled)
        {
            _speed = originalSpeed;
            Debug.Log("Release Attack Timer: " + _basicAttackTimer);
            if (_basicAttackTimer >= _basicAttackTime)
            {
                Debug.Log("Basic attack reached");
                normalAttack.Invoke();
            }

            if (attackChargeBar != null)
            {
                attackChargeBar.gameObject.SetActive(false);
            }
        }
    }

    public IEnumerator KillBall(GameObject currentOrb)
    {
        yield return new WaitForSeconds(_ballLifetime);
        if (heldOrb == currentOrb)
        {
            hasOrb = false;
            Destroy(heldOrb);
            heldOrb = null;
        }
    }

    public void StartKillBall(GameObject currentOrb)
    {
        StartCoroutine(KillBall(currentOrb));
    }

    public virtual void OnAbility1(InputAction.CallbackContext context)
    {

    }

    public virtual void OnAbility2(InputAction.CallbackContext context)
    {

    }

    public virtual void OnDebug(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TakeDamage(50.0f);
        }
    }

    public void OnDissconect()
    {
        GameManager.Instance.DisconnectPlayer(this);
        Destroy(gameObject);
    }

    public IEnumerator DoBallAttackHaptics()
    {
        if (playerGamepad != null)
        {
            playerGamepad.SetMotorSpeeds(1.0f, 1.0f);
            yield return new WaitForSeconds(1000.0f);
            playerGamepad.ResetHaptics();
        }
    }

    public IEnumerator AdjustableHaptics(float lowInput, float highInput, float time)
    {
        if (playerGamepad != null)
        {
            playerGamepad.SetMotorSpeeds(lowInput, highInput);
            yield return new WaitForSeconds(time);
            playerGamepad.ResetHaptics();
        }
    }
}