using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private CharacterBase ownerPlayer;

    public void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.GetComponent<CharacterBase>() != ownerPlayer)
        {
            CharacterBase player = collision.gameObject.GetComponent<CharacterBase>();
            player.TakeDamage(damage, damageEffect, effectTime);
        }

        Destroy(gameObject);
    }

    public void SetOwner(CharacterBase inputPlayer)
    {
        ownerPlayer = inputPlayer;
    }
}
