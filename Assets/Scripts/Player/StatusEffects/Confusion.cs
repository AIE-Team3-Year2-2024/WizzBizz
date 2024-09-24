using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confusion : MonoBehaviour
{
    [HideInInspector]
    public float lifeTime;

    private CharacterBase _player;

    /// <summary>
    /// manage lifetime and whther it is enabled
    /// </summary>
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime < 0 )
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
        _player.confused = true;
    }

    /// <summary>
    /// set the player back to normal
    /// </summary>
    private void OnDisable()
    {
        _player.confused = false;
    }
}
