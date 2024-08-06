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

    [Tooltip("the players health")]
    [SerializeField]
    private float _health;

    [Tooltip("whether or not on move will be skipped")]
    public bool canMove;

    [Tooltip("this value multiplies the size of the pointer aimer when aiming")]
    [SerializeField]
    private float _pointerAimerRange;

    public float currentAimMagnitude;

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

    // Update is called once per frame
    void Update()
    {
        transform.position += _movementDirection * _speed * Time.deltaTime;
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


    public void OnAim(InputAction.CallbackContext context)
    {
        Vector3 aimDirection = new Vector3();
        aimDirection.z = context.ReadValue<Vector2>().y;
        aimDirection.x = context.ReadValue<Vector2>().x;
        transform.LookAt(aimDirection += transform.position, transform.up);

        currentAimMagnitude = context.ReadValue<Vector2>().magnitude;

        _pointerAimer.localScale = new Vector3(_pointerAimer.localScale.x, 1 + (_pointerAimerRange * currentAimMagnitude), _pointerAimer.localScale.z);
    }

    public void OnDash(InputAction.CallbackContext context)
    {

    }

    public void OnCatch(InputAction.CallbackContext context)
    {

    }

    public void TakeDamage(float damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            Debug.Log(gameObject.name + " is me and i am dead");
        }
    }

    public void TakeDamage(float damage, StaitisEffects effect, float time)
    {
        _health -= damage;

        if (_health <= 0)
        {
            Debug.Log(gameObject.name + " is me and i am dead");
        }
    }

    public virtual void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
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

    public void OnDissconect()
    {
        GameManager.Instance.DisconnectPlayer(this);
        Destroy(gameObject);
    }

    public IEnumerator DoBallAttackHaptics()
    {
        playerGamepad.SetMotorSpeeds(1.0f, 1.0f);
        yield return new WaitForSeconds(1000.0f);
        playerGamepad.ResetHaptics();
    }
}