using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _orbCollectiblePrefab;

    [SerializeField] private float _coolDown = 3.0f;

    private GameObject _spawnedCollectible = null;
    private float _timer = 0.0f;
    private CharacterBase _playerWhoCollected = null;
    private bool _collected = false;
    private bool _justCollected = true;

    void Start()
    {
        Reset();
    }

    void Update()
    {
        if (_justCollected == true)
            return;

        if (_collected == true)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0.0f)
            {
                Reset();
            }
        }
    }

    public void Reset()
    {
        if (_spawnedCollectible)
        {
            Destroy(_spawnedCollectible);
            _spawnedCollectible = null;
        }

        _spawnedCollectible = Instantiate(_orbCollectiblePrefab, transform);
        _spawnedCollectible.GetComponent<OrbCollectible>().spawner = this;
        _timer = _coolDown;
        _collected = false;
    }

    public void Collect(CharacterBase playerRef)
    {
        _collected = true;
        _justCollected = true;
        _spawnedCollectible = null;
        _playerWhoCollected = playerRef;
    }

    public void OnTriggerExit(Collider other)
    {
        if (other?.GetComponent<CharacterBase>() == _playerWhoCollected)
        {
            _justCollected = false;
            _playerWhoCollected = null;
        }
    }
}
