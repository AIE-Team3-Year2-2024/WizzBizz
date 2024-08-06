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

    [Tooltip("the speed this character will move at")]
    [SerializeField]
    private float _speed;

    private float _origanalSpeed;

    [Tooltip("how long this character will dash for")]
    [SerializeField]
    private float _dashTime;

    [Tooltip("the speed this charcater will dash at")]
    [SerializeField]
    private float _dashSpeed;

    [Tooltip("the speed the player will move at if chgarging ann attack")]
    [SerializeField]
    private float _chargeSpeed;

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

    public enum StaitisEffects
    {
        NONE,
        SLOW
    }

    private void Start()
    {
        _origanalSpeed = _speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _movementDirection * _speed * Time.deltaTime;
        _basicAttackTimer += Time.deltaTime;
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
        float oldSpeed = _speed;
        _speed = _dashSpeed;
        _movementDirection = transform.forward;
        yield return new WaitForSeconds(_dashTime);
        canMove = true;
        _speed = oldSpeed;
        _movementDirection = Vector3.zero;
    }

    public void OnCatch(InputAction.CallbackContext context)
    {

    }

    public void TakeDamage(float damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            Death();
        }
    }

    public void TakeDamage(float damage, StaitisEffects effect, float time)
    {
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
            _speed = _origanalSpeed;
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