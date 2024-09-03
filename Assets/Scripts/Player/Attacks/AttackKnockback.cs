using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackKnockback : MonoBehaviour
{
    [SerializeField]
    private float knockbackAmount = 3.0f;

    public void DoKnockback(Collision coll, CharacterBase player)
    {
        if (coll.contactCount > 0)
        {
            Vector3 knockBackDir = coll.GetContact(0).normal;
            player.TakeKnockback(knockbackAmount, knockBackDir);
        }
    }
}
