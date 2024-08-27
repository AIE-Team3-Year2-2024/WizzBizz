using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weakness : MonoBehaviour
{
    [HideInInspector]
    public float lifeTime = 0;

    [Tooltip("how much will be taken off of the players damage multiplyer (players base damage multiplyer is 1)")]
    [SerializeField]
    private float _damgeMinus;

    private CharacterBase _player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0 )
        {
            enabled = false;
        }
    }

    private void OnEnable()
    {
        if(_player == null)
        {
            _player = GetComponent<CharacterBase>();
        }
        _player.damageMult -= _damgeMinus;
    }

    private void OnDisable()
    {
        _player.damageMult += _damgeMinus;
    }
}
