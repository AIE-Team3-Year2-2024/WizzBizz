using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weakness : MonoBehaviour
{
    [HideInInspector]
    public float lifeTime = 0;

    [Tooltip("how much will be taken off of the players damage multiplyer (players base damage multiplyer is 1)")]
    [HideInInspector]
    public float damageMinus;

    private CharacterBase _player;

    /// <summary>
    /// manage lifetime and whther it is enabled
    /// </summary>
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0 )
        {
            enabled = false;
        }
    }

    /// <summary>
    /// apply effect to player
    /// </summary>
    private void OnEnable()
    {
        if(_player == null)
        {
            _player = GetComponent<CharacterBase>();
        }
        _player.damageMult -= damageMinus;
    }

    /// <summary>
    /// set the player back to normal
    /// </summary>
    private void OnDisable()
    {
        _player.damageMult += damageMinus;
    }
}
