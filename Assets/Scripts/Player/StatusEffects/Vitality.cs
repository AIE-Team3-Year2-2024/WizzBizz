using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vitality : MonoBehaviour
{
    [HideInInspector]
    public float lifeTime = 0;

    [Tooltip("how much will be added too the players damage multiplyer (players base damage multiplyer is 1)")]
    [SerializeField]
    private float _damgePlus;

    private CharacterBase _player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
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
        _player.damageMult += _damgePlus;
    }

    private void OnDisable()
    {
        _player.damageMult -= _damgePlus;
    }
}
