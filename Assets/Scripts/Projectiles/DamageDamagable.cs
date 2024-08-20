using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageDamagable : MonoBehaviour
{
    [Tooltip("the amount of damge this object will deal to a player or object")]
    [SerializeField]
    private float damage;

    [Tooltip("whether the object will destroy itself on collision")]
    [SerializeField]
    private bool destroyOnCollision;

    [Tooltip("event invoked in OnDestroy")]
    public UnityEvent DoOnDestroy;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Damagable>())
        {
            Damagable damageObject = collision.gameObject.GetComponent<Damagable>();
            damageObject.TakeDamage(damage);
        }

        if (destroyOnCollision)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        DoOnDestroy.Invoke();
    }
}
