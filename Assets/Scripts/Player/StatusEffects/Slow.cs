using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : MonoBehaviour
{
    [HideInInspector]
    public float lifeTime = 0;

    [Tooltip("how much will be taken off of the players speed")]
    [SerializeField]
    private float _speedMinus;

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
        _player.AddSpeed(-_speedMinus);
    }

    /// <summary>
    /// set the player back to normal
    /// </summary>
    private void OnDisable()
    {
        _player.AddSpeed(_speedMinus);
    }
}
