using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class DamagePlayerOnCollision : MonoBehaviour
{
    [Tooltip("the amount of damge this object will deal to a player or object")]
    public float damage;

    [Tooltip("the damage type of this attack")]
    [SerializeField]
    private CharacterBase.StatusEffects damageEffect;

    [Tooltip("how long this attacks effect lasts for")]
    [SerializeField]
    private float effectTime;

    [Tooltip("whether the object will destroy itself on any collision")]
    [SerializeField]
    private bool destroyOnCollision;

    [Tooltip("whether the object will destroy itself when it hits a player")]
    [SerializeField]
    private bool destroyOnPlayerCollision;

    [Tooltip("event invoked in OnDestroy")]
    public UnityEvent DoOnDestroy;

    [Tooltip("event invoked when a player is hit")]
    public UnityEvent DoOnHit;

    [Tooltip("the component that allows this projectiloe direction to be controlled by the player")]
    [SerializeField]
    private ControlProjectileDirection controlComponent;

    [Tooltip("any damage player components in this array will have their player set to the player set to tthis component (the player set to this component is the one this cannot damage)")]
    [SerializeField]
    private DamagePlayerOnCollision[] damageChildren;

    private CharacterBase ownerPlayer;

    private AttackKnockback _knockbackComponent;

    private void Awake()
    {
        _knockbackComponent = GetComponent<AttackKnockback>();
    }

    /// <summary>
    /// if this hits a player this isnt the owner do damage to them
    /// </summary>
    /// <param name="collision"></param>
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<CharacterBase>() == ownerPlayer)
        {
            return;
        }

        if (collision.gameObject.GetComponent<CharacterBase>())
        {
            CharacterBase player = collision.gameObject.GetComponent<CharacterBase>();
            player.TakeDamage(damage, damageEffect, effectTime);
            DoOnHit.Invoke();

            if (_knockbackComponent)
            {
                _knockbackComponent.DoKnockback(collision, player);
            }
            if(destroyOnPlayerCollision)
            {
                Destroy(gameObject);
            }

        }

        if (destroyOnCollision)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// if this hits a player this isnt the owner do damage to them
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<CharacterBase>() == ownerPlayer)
        {
            return;
        }

        if (collision.gameObject.GetComponent<CharacterBase>())
        {
            CharacterBase player = collision.gameObject.GetComponent<CharacterBase>();
            Debug.Log("hit player with trigger");
            player.TakeDamage(damage, damageEffect, effectTime);
            DoOnHit.Invoke();
            if (destroyOnPlayerCollision)
            {
                Destroy(gameObject);
            }
        }

        if (destroyOnCollision)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// if this hits a player this isnt the owner do damage to them
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.GetComponent<CharacterBase>() == ownerPlayer)
        {
            return;
        }

        if (collision.gameObject.GetComponent<CharacterBase>())
        {
            CharacterBase player = collision.gameObject.GetComponent<CharacterBase>();
            player.TakeDamage(damage, damageEffect, effectTime);
            DoOnHit.Invoke();
            if (destroyOnPlayerCollision)
            {
                Destroy(gameObject);
            }
        }

        if (destroyOnCollision)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// get the player who made this object so this connot damage them
    /// </summary>
    /// <param name="inputPlayer"></param>
    public void SetOwner(CharacterBase inputPlayer)
    {
        ownerPlayer = inputPlayer;

        foreach (DamagePlayerOnCollision child in damageChildren)
        {
            child.ownerPlayer = inputPlayer;
        }


        if (controlComponent)
        {
            ownerPlayer.GetComponent<PlayerInput>().actions.FindAction("Aim").performed += controlComponent.OnAim;
            ownerPlayer.GetComponent<PlayerInput>().actions.FindAction("Aim").canceled += controlComponent.OnAim;
        }
    }


    private void OnDestroy()
    {
        if (controlComponent)
        {
            ownerPlayer.GetComponent<PlayerInput>().actions.FindAction("Aim").performed -= controlComponent.OnAim;
            ownerPlayer.GetComponent<PlayerInput>().actions.FindAction("Aim").canceled -= controlComponent.OnAim;
        }
        DoOnDestroy.Invoke();
    }
}
