using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;

public abstract class CharacterBase : MonoBehaviour
{
    protected bool hasOrb;
    [Tooltip("the speed this character will move at")]
    [SerializeField]
    private float speed;

    [Tooltip("the players health")]
    [SerializeField]
    private float _health;

    [Tooltip("whether or not on move will be skipped")]
    [SerializeField]
    public bool canMove;

    [Tooltip("this value multiplies the size of the pointer aimer when aiming")]
    [SerializeField]
    private float pointerAimerRange;

    [Tooltip("the image used to show where the player is aiming")]
    [SerializeField]
    private RectTransform pointerAimer;

    private Vector3 _movementDirection;

    [Tooltip("where to spawn projectiles on this character")]
    [SerializeField]
    protected Transform projectileSpawnPosition;

    public enum StaitisEffects
    {
        SLOW
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _movementDirection * speed * Time.deltaTime;
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

        pointerAimer.localScale = new Vector3(pointerAimer.localScale.x, 1 + (pointerAimerRange * context.ReadValue<Vector2>().magnitude), pointerAimer.localScale.z);
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

    }


    public virtual void OnAbility1(InputAction.CallbackContext context)
    {

    }

    public virtual void OnAbility2(InputAction.CallbackContext context)
    {

    }
}
