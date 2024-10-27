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

    private bool _invincible = false;

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

    [Tooltip("this will be the height of the pointer aimer when the aim stick is not aiming")]
    [SerializeField]
    private float _defualtPointerAimerHeight;

    [HideInInspector]
    public float currentAimMagnitude;

    [Tooltip("the amount of time needed to charge the ball attack")]
    [SerializeField]
    private float _ballAttackTime;

    [Tooltip("the amount of time needed to charge the basic attack")]
    [SerializeField]
    private float _basicAttackTime;

    [HideInInspector]
    public float damageMult = 1;

    private float _ballAttackTimer = 0;

    [Tooltip("how long it takes for the basic attack to cool down")]
    [SerializeField]
    private float _basicAttackCooldownGoal;

    private float _basicAttackCooldown = 0;

    [Tooltip("the image used to show where the player is aiming")]
    [SerializeField]
    private RectTransform _pointerAimer;

    [Tooltip("the slider component of this players health bar")]
    [SerializeField]
    private Slider healthBar;

    [Tooltip("The slider component of the ball attack charge up bar.")]
    [SerializeField]
    private Slider ballAttackChargeBar;

    [Tooltip("The slider component of the basic attack cool down bar.")]
    [SerializeField]
    private Slider basicAttackCoolDownBar;

    [Tooltip("the Text on the player showing what number they are")]
    public TMP_Text playerNumber;

    private Vector3 _movementDirection;

    [Tooltip("where to spawn projectiles on this character")]
    public Transform _projectileSpawnPosition;

    [SerializeField, Tooltip("the pause screen object to create on pause")]
    private GameObject _pauseScreen;

    [Tooltip("the active pause screen object is stored here so it can be destroyed")]
    private GameObject _currentPauseScreen;

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

    /// <summary>
    /// initalization
    /// </summary>
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
            healthBar.value = _health;
        }

        rb = GetComponent<Rigidbody>();

        damageMult = 1;
    }

    /// <summary>
    /// handles timers and ui for the basic and ball attacks
    /// </summary>
    void Update()
    {
        _ballAttackTimer += Time.deltaTime;
        _basicAttackCooldown += Time.deltaTime;

        if (ballAttackChargeBar != null)
        {
            ballAttackChargeBar.maxValue = _ballAttackTime;
            ballAttackChargeBar.value = _ballAttackTimer;
        }

        if(basicAttackCoolDownBar != null)
        {
            basicAttackCoolDownBar.maxValue = _basicAttackCooldownGoal;
            basicAttackCoolDownBar.value = _basicAttackCooldown;
        }
    }

    /// <summary>
    /// handles the players movement
    /// </summary>
    void FixedUpdate()
    {
        _acceleration = _speed * _deceleration;
        _movementDirection.y = 0.0f;
        if (_movementDirection.magnitude > 0.0f)
            rb.AddForce(_movementDirection.normalized * _acceleration, ForceMode.VelocityChange);
        //_velocity += _movementDirection * _acceleration; // Add acceleration when there is input.

        //if (rb.velocity.magnitude > 0.0f && _movementDirection.magnitude <= 0.0f) // Only start decelerating when the character is moving, but also when there's no input.
        Vector3 newVelocity = rb.velocity;
        newVelocity *= (1.0f - _deceleration);
        newVelocity.y = rb.velocity.y;
        rb.velocity = newVelocity;
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
            _movementDirection = -_movementDirection;//reverses the movement direction
        }
    }

    /// <summary>
    /// sets _movementDirection to 0
    /// </summary>
    public void CancelPlayerMovement()
    {
        _movementDirection = Vector3.zero;
    }

    /// <summary>
    /// adds addition to both speed and origanal speed
    /// </summary>
    /// <param name="addition"></param>
    public void AddSpeed(float addition)
    {
        _speed += addition;
        originalSpeed += addition;
    }

    /// <summary>
    /// changes speed to newSpeed
    /// </summary>
    /// <param name="newSpeed"></param>
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
            aimDirection = -aimDirection;//reverses aim direction
        }
                
        transform.LookAt(aimDirection += transform.position, transform.up);
        

        currentAimMagnitude = context.ReadValue<Vector2>().magnitude;

        _pointerAimer.sizeDelta = new Vector2(_pointerAimer.sizeDelta.x, _defualtPointerAimerHeight + ((_pointerAimerRange - _defualtPointerAimerHeight) * currentAimMagnitude));
    }

    /// <summary>
    /// this function is callked by the player input system when the dash button is pressed
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
        canDash = false;
        _movementDirection = Vector3.zero;
        StartCoroutine(WaitToDash());
    }

    /// <summary>
    /// re enables dashing after _dashWaitTime
    /// </summary>
    /// <returns></returns>
    public IEnumerator WaitToDash()
    {
        yield return new WaitForSeconds(_dashWaitTime);
        canDash = true;
    }

    /// <summary>
    /// called by the player input system when a player attempts to catch
    /// </summary>
    /// <param name="context"></param>
    public void OnCatch(InputAction.CallbackContext context)
    {
        StartCoroutine(CatchRoutine());
    }

    /// <summary>
    /// takes away movement but activates the catching trigger and then reverses that
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// takes damge from helth and updates the helath bar and calls death if helath is under 0
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        if(_health <= 0 || _invincible)
        {
            return;
        }
        _health -= damage;

        if (healthBar)
        {
            healthBar.value = _health;
        }

        //makes this player drop the orb if they have it
        if (hasOrb)
        {
            hasOrb = false;
            Destroy(heldOrb);
            heldOrb = null;
        }

        if (_health <= 0)
        {
            Death();
        }
    }

    /// <summary>
    /// handles taking damage and call death and turns on status affect components 
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="effect"></param>
    /// <param name="time"></param>
    public void TakeDamage(float damage, StatusEffects effect, float time)
    {
        if (_health <= 0 || _invincible)
        {
            return;
        }
        _health -= damage;

        //makes this player drop the orb if they have it
        if (hasOrb)
        {
            hasOrb = false;
            Destroy(heldOrb);
            heldOrb = null;
        }

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

                    _confusion.enabled = true;
                    _confusion.lifeTime = time;
                    
                    break;
                }

            case (StatusEffects.DISABLED):
                {
                    if(_silence.enabled)
                    {
                        _silence.enabled = false;
                    }
                    _disabled.enabled = true;
                    _disabled.lifeTime = time;
                    break;
                }

            case (StatusEffects.SILENCE):
                {
                    if(_disabled.enabled)
                    {
                        _disabled.enabled = false;
                    }
                    _silence.enabled = true;
                    _silence.lifeTime = time;
                    
                    break;
                }

            case (StatusEffects.CRIPPLED):
                {

                    _crippled.enabled = true;
                    _crippled.lifeTime = time;
                    
                    break;
                }

            case (StatusEffects.STUN):
                {

                    _stun.enabled = true;
                    _stun.lifeTime = time;
                    
                    break;
                }

            case (StatusEffects.WEAKNESS):
                {
                    _weakness.enabled = true;
                    _weakness.lifeTime = time;
                    break;
                }

            case (StatusEffects.VITALITY):
                {
                    _vitality.enabled = true;
                    _vitality.lifeTime = time;
                    
                    break;
                }

            case (StatusEffects.SLOW):
                {
                    _slow.enabled = true;
                    _slow.lifeTime = time;
                    
                    break;
                }

            case (StatusEffects.HASTE):
                {

                    _haste.enabled = true;
                    _haste.lifeTime = time;
                    break;
                }

            case (StatusEffects.BURNING):
                {
                    _burn.enabled = true;
                    _burn.lifeTime = time;
                    
                    break;
                }

            case (StatusEffects.POISON):
                {
                    _poison.enabled = true;
                    _poison.lifeTime = time;
                    
                    break;
                }

            case (StatusEffects.CURE):
                {
                    _cure.enabled = true;
                    _cure.lifeTime = time;
                    break;
                }

            case (StatusEffects.DEMENTIA):
                {
                    _dementia.enabled = true;
                    _dementia.lifeTime = time;
                    break;
                }

        }
    }

    /// <summary>
    /// makes the player unable to take damage for time
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator InvincibiltyForTime(float time)
    {
        _invincible = true;
        yield return new WaitForSeconds(time);
        _invincible = false;
    }

    /// <summary>
    /// makes the player unable to take damge for as many frames as frames is equal to
    /// </summary>
    /// <param name="frames"></param>
    /// <returns></returns>
    public IEnumerator InvincibilityForFrames(int frames)
    {
        _invincible = true;
        for(int i = 0; i > frames; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        _invincible = false;
    }

    /// <summary>
    /// adds force to the rigidbody in the opposite of dir by amount
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="dir"></param>
    public void TakeKnockback(float amount, Vector3 dir)
    {
        if (dir != Vector3.zero && amount > 0.0f)
        {
            rb.AddForce(-dir * amount, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// stop movement and update the game manger on the players death
    /// </summary>
    public void Death()
    {
        input.DeactivateInput();
        _movementDirection = Vector3.zero;
        GameManager.Instance.PlayerDeath(this);
    }

    /// <summary>
    /// this is called by the player input system and checks whther the ball or basic attck should be invoked
    /// </summary>
    /// <param name="context"></param>
    public virtual void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _ballAttackTimer = 0;
            
            if (!hasOrb)
            {
                if (_basicAttackCooldown >= _basicAttackCooldownGoal)
                {
                    normalAttack.Invoke();
                    _basicAttackCooldown = 0;
                }
            }
            else
            {
                if (ballAttackChargeBar != null)
                {
                    ballAttackChargeBar.gameObject.SetActive(true); 
                }
                _speed = _chargeSpeed;
            }
            
        }
        else if (context.canceled)
        {
            _speed = originalSpeed;
            if (hasOrb)
            {
                if (_ballAttackTimer >= _ballAttackTime)
                {
                    StartCoroutine(DoBallAttackHaptics());
                    StopCoroutine(KillBall(null));
                    ballAttack.Invoke();
                    Destroy(heldOrb);
                    heldOrb = null;
                    hasOrb = false;
                }
            }

            if (ballAttackChargeBar != null)
            {
                ballAttackChargeBar.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// called by the player input system to pause the game and take controll of the menu
    /// </summary>
    /// <param name="context"></param>
    public void OnPause(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            GameManager.Instance.Pause(this);
            input.SwitchCurrentActionMap("UI");
            _currentPauseScreen = Instantiate(_pauseScreen);
        }
    }

    /// <summary>
    /// called by the player input system to unpause the game and bring this player back to normal controlls
    /// </summary>
    /// <param name="context"></param>
    public void OnUnPause(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            GameManager.Instance.UnPause(this);
            input.SwitchCurrentActionMap("Player");
            if (_currentPauseScreen != null)
            {
                Destroy(_currentPauseScreen);
                _currentPauseScreen = null;
            }
        }
    }

    /// <summary>
    /// called to unpause the game
    /// </summary>
    public void UnPause()
    {
        GameManager.Instance.UnPause(this);
        input.SwitchCurrentActionMap("Player");
        if (_currentPauseScreen != null)
        {
            Destroy(_currentPauseScreen);
            _currentPauseScreen = null;
        }
    }

    /// <summary>
    /// destroyes the held ball after ball lifetime
    /// </summary>
    /// <param name="currentOrb"></param>
    /// <returns></returns>
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

    public virtual void OnDebug(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TakeDamage(50.0f);
        }
    }

    /// <summary>
    /// handles dissconnecting the player by destroying them and updating the game manger to remove them
    /// </summary>
    public void OnDissconect()
    {
        GameManager.Instance.DisconnectPlayer(this);
        Destroy(gameObject);
    }

    /// <summary>
    /// vibrates this players controller
    /// </summary>
    /// <returns></returns>
    public IEnumerator DoBallAttackHaptics()
    {
        if (playerGamepad != null)
        {
            playerGamepad.SetMotorSpeeds(1.0f, 1.0f);
            yield return new WaitForSeconds(1000.0f);
            playerGamepad.ResetHaptics();
        }
    }

    /// <summary>
    /// vibrates the players controller based off of the inputs
    /// </summary>
    /// <param name="lowInput"></param>
    /// <param name="highInput"></param>
    /// <param name="time"></param>
    /// <returns></returns>
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