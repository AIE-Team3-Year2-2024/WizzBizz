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
    [SerializeField]
    private float health;

    private float currentIntervalAmount = 0;

    private CharacterBase _player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
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

    private void OnEnable()
    {
        if (_player == null)
        {
            _player = GetComponent<CharacterBase>();
        }
    }

    private void OnDisable()
    {
        currentIntervalAmount = 0;
    }
}
