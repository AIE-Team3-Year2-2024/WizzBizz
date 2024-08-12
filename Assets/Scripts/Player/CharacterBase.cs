using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;

public class CharacterBase : MonoBehaviour
{
    [HideInInspector] public bool hasOrb = false;
    [HideInInspector] public GameObject heldOrb = null;

    [HideInInspector] public Gamepad playerGamepad = null;

    private PlayerInput input;

    [Tooltip("The maximum speed this character will move at.")]
    [SerializeField]
    private float _speed;

    private float _originalSpeed;

    [SerializeField]
    [Tooltip("The rate at which the character will accelerate towards the max speed.")]
    private float _acceleration;
    [Tooltip("The amount the character will decelerate when it is not moving. This is a percentage (0-1).")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float _deceleration;

    private float _originalAccel;
    private float _originalDecel;

    private Vector3 _velocity;

    private bool _shouldStopSliding = false;

    [Tooltip("this is the trigger collider used for catching")]
    public Collider catchTrigger;

    [Tooltip("how long this character will dash for")]
    [SerializeField]
    private float _dashTime;

    [Tooltip("the speed this charcater will dash at")]
    [SerializeField]
    private float _dashSpeed;

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

    [Tooltip("whether or not on move will be skipped")]
    [HideInInspector]
    public bool canMove = true;


    [Tooltip("this value multiplies the size of the pointer aimer when aiming")]
    [SerializeField]
    private float _pointerAimerRange;

    [HideInInspector]
    public float currentAimMagnitude;

    [Tooltip("the amount of time needed to charge the basic attack")]
    [SerializeField]
    private float _basicAttackTime;

    private float _basicAttackTimer = 0;

    [Tooltip("the image used to show where the player is aiming")]
    [SerializeField]
    private RectTransform _pointerAimer;

    private Vector3 _movementDirection;

    [Tooltip("where to spawn projectiles on this character")]
    public Transform _projectileSpawnPosition;

    [Header("Trigger Attacks")]
    public UnityEvent ballAttack;
    public UnityEvent normalAttack;

    private Rigidbody rb;

    public enum StaitisEffects
    {
        NONE,
        SLOW
    }

    private void Start()
    {
        input = GetComponent<PlayerInput>();
        catchTrigger.isTrigger = true;
        catchTrigger.enabled = false;

        _originalSpeed = _speed;
        _originalAccel = _acceleration;
        _originalDecel = _deceleration;

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _basicAttackTimer += Time.deltaTime;   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_movementDirection.magnitude > 0.0f)
            rb.AddForce(_movementDirection * _acceleration, ForceMode.Impulse);
        //_velocity += _movementDirection * _acceleration; // Add acceleration when there is input.

        if (rb.velocity.magnitude > 0.0f && _movementDirection.magnitude <= 0.0f) // Only start decelerating when the character is moving, but also when there's no input.
            rb.velocity *= (1.0f - _deceleration);
        //_velocity *= (1.0f - _deceleration); // Invert the value so it's more intuitive in the inspector, so 0 is no deceleration instead of 1.

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, _speed);

        //_velocity = Vector3.ClampMagnitude(_velocity, _speed); // Clamp the velocity to the maximum speed.
        //rb.position += _velocity * Time.fixedDeltaTime; // Apply the velocity to the character position.
        //rb.velocity = _velocity;
    }

    void LateUpdate()
    {
        if (_velocity.magnitude <= 0.0f || _movementDirection.magnitude > 0.0f) // If the character has stopped moving, or if there is input.
        {
            if (_shouldStopSliding) // The player should stop sliding now.
            {
                _acceleration = _originalAccel; // Reset.
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
        if (context.started)
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
        canMove = false;
        _speed = _dashSpeed;
        _movementDirection = transform.forward;
        yield return new WaitForSeconds(_dashTime);
        canMove = true;
        _speed = _originalSpeed;
        _movementDirection = Vector3.zero;
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
        _acceleration = _originalAccel * (slipperyness * accelFactor);
        _deceleration = inv;
    }

    public void StopSliding()
    {
        _shouldStopSliding = true;
    }

    public void TakeDamage(float damage)
    {
        if(_health <=0)
        {
            return;
        }
        _health -= damage;

        if (_health <= 0)
        {
            Death();
        }
    }

    public void TakeDamage(float damage, StaitisEffects effect, float time)
    {
        if (_health <= 0)
        {
            return;
        }
        _health -= damage;

        if (_health <= 0)
        {
            Death();
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
        if (context.started)
        {
            _basicAttackTimer = 0;
            if (hasOrb)
            {
                StartCoroutine(DoBallAttackHaptics());

                ballAttack.Invoke();
                Destroy(heldOrb);
                heldOrb = null;
                hasOrb = false;
            }
            else
            {
                _speed = _chargeSpeed;
            }
        }
        else if (context.canceled)
        {
            _speed = _originalSpeed;
            if (_basicAttackTimer >= _basicAttackTime)
            {
                normalAttack.Invoke();
            }
        }
    }

    public virtual void OnAbility1(InputAction.CallbackContext context)
    {

    }

    public virtual void OnAbility2(InputAction.CallbackContext context)
    {

    }

    public virtual void OnDebug(InputAction.CallbackContext context)
    {
        if (context.started)
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
}