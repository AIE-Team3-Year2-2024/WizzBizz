using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

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

    [Tooltip("whether the object will destroy itself on collision")]
    [SerializeField]
    private bool destroyOnCollision;

    [Tooltip("event invoked in OnDestroy")]
    public UnityEvent DoOnDestroy;

    [Tooltip("event invoked when a player is hit")]
    public UnityEvent DoOnHit;

    private CharacterBase ownerPlayer;

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
        }

        if (destroyOnCollision)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
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
        }

        if (destroyOnCollision)
        {
            Destroy(gameObject);
        }
    }

    public void SetOwner(CharacterBase inputPlayer)
    {
        ownerPlayer = inputPlayer;
        DamagePlayerOnCollision[] children = transform.GetComponentsInChildren<DamagePlayerOnCollision>();

        foreach (DamagePlayerOnCollision child in children)
        {
            child.SetOwner(inputPlayer);
        }
    }

    private void OnDestroy()
    {
        DoOnDestroy.Invoke();
    }
}
