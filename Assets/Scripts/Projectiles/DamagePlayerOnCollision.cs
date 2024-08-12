using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class DamagePlayerOnCollision : MonoBehaviour
{
    [Tooltip("the amount of damge this object will deal to a player or object")]
    [SerializeField]
    private float damage;

    [Tooltip("the damage type of this attack")]
    [SerializeField]
    private CharacterBase.StaitisEffects damageEffect;

    [Tooltip("how long this attacks effect lasts for")]
    [SerializeField]
    private float effectTime;

    [Tooltip("whether the object will destroy itself on collision")]
    [SerializeField]
    private bool destroyOnCollision;

    [Tooltip("event invoked in OnDestroy")]
    public UnityEvent DoOnDestroy;

    private CharacterBase ownerPlayer;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<CharacterBase>() != ownerPlayer && collision.gameObject.GetComponent<CharacterBase>())
        {
            CharacterBase player = collision.gameObject.GetComponent<CharacterBase>();
            player.TakeDamage(damage, damageEffect, effectTime);
        }

        if (destroyOnCollision)
        {
            Destroy(gameObject);
        }
    }

    public void SetOwner(CharacterBase inputPlayer)
    {
        ownerPlayer = inputPlayer;
    }

    private void OnDestroy()
    {
        DoOnDestroy.Invoke();
    }
}
