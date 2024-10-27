using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cure : MonoBehaviour
{
    [HideInInspector]
    public float lifeTime = 0;

    [Tooltip("how often to apply health to this player")]
    [SerializeField]
    private float _healthInterval;

    [Tooltip("how much health the player player wil take after the interval")]
    [HideInInspector]
    public float health;

    private float currentIntervalAmount = 0;

    private CharacterBase _player;
    
    /// <summary>
    /// keep track of life time and healing the player on an interval
    /// </summary>
    void Update()
    {
        lifeTime -= Time.deltaTime;
        currentIntervalAmount += Time.deltaTime;

        if (currentIntervalAmount > _healthInterval)
        {
            _player.TakeDamage(-health);
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
