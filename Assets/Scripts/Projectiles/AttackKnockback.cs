using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackKnockback : MonoBehaviour
{
    [SerializeField]
    private float knockbackAmount = 3.0f;

    [Header("AB Test, switches the knockback direction from either A: the direction of the contact (think of pool), or B: the direction of the projectile.")]
    [SerializeField]
    private bool abTest = false;

    public void DoKnockback(Collision coll, CharacterBase player)
    {
        if (coll.contactCount > 0)
        {
            Vector3 knockBackDir = (abTest == false) ? coll.GetContact(0).normal : -transform.forward;
            player.TakeKnockback(knockbackAmount, knockBackDir);
        }
    }
}
