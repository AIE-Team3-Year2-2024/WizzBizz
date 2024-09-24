using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : MonoBehaviour
{
    [HideInInspector]
    public float lifeTime = 0;

    [Tooltip("how often to apply damage to this player")]
    [SerializeField]
    private float _damageInterval;

    [Tooltip("how much damage the player player wil take after the interval")]
    [SerializeField]
    private float damage;

    private float currentIntervalAmount = 0;

    private CharacterBase _player;

    /// <summary>
    /// keep track of life time and damaging the player on an interval
    /// </summary>
    void Update()
    {
        lifeTime -= Time.deltaTime;
        currentIntervalAmount += Time.deltaTime;

        if (currentIntervalAmount > _damageInterval)
        {
            _player.TakeDamage(damage);
            currentIntervalAmount = 0;
        }

        if (lifeTime < 0)
        {
            enabled = false;
        }
    }

    /// <summary>
    /// find player
    /// </summary>
    private void OnEnable()
    {
        if (_player == null)
        {
            _player = GetComponent<CharacterBase>();
        }
    }

    /// <summary>
    /// set the player back to normal
    /// </summary>
    private void OnDisable()
    {
        currentIntervalAmount = 0;
    }
}
