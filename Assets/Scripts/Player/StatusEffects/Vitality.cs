using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vitality : MonoBehaviour
{
    [HideInInspector]
    public float lifeTime = 0;

    [Tooltip("how much will be added too the players damage multiplyer (players base damage multiplyer is 1)")]
    [HideInInspector]
    public float damagePlus;

    private CharacterBase _player;

    /// <summary>
    /// manage lifetime and whther it is enabled
    /// </summary>
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            enabled = false;
        }
    }

    /// <summary>
    /// apply effect to player
    /// </summary>
    private void OnEnable()
    {
        if (_player == null)
        {
            _player = GetComponent<CharacterBase>();
        }
        _player.damageMult += damagePlus;
    }

    /// <summary>
    /// set the player back to normal
    /// </summary>
    private void OnDisable()
    {
        _player.damageMult -= damagePlus;
    }
}
