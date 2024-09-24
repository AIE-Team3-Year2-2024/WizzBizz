using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpikeTile : MonoBehaviour
{
    private float _playerCollideTimer = 0;

    [Tooltip("how long the player has to be on this tile before damaging")]
    [SerializeField]
    private float _hitTime;

    [Tooltip("how much damage the player will take")]
    [SerializeField]
    private float _damage;

    [Tooltip("event invoked when damging the player")]
    public UnityEvent doWhenDamaging;

    /// <summary>
    /// will decrease _playerCollideTimer down to zero
    /// </summary>
    void Update()
    {
        _playerCollideTimer -= Time.deltaTime;
        if(_playerCollideTimer < 0)
        {
            _playerCollideTimer = 0;
        }
    }

    /// <summary>
    /// will increase _playerCollidide timer of colliding qwith a playre and if it reaches hit time will find the player and damage it
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerStay(Collider other)
    {
        CharacterBase character;
        if(character = other.GetComponent<CharacterBase>())
        {
            _playerCollideTimer += Time.deltaTime * 2;

            if (_playerCollideTimer >= _hitTime)
            {
                character.TakeDamage(_damage);
                _playerCollideTimer = 0;
            }
        }
        
    }

}
