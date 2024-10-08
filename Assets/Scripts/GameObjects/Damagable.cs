using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    [Tooltip("how much damage this object can take before it is destroyed")]
    public float health;

    /// <summary>
    /// this will minus this objects health and destroy it if the health is under 0
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
