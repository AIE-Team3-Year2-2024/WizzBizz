using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _orbCollectiblePrefab;

    [SerializeField] private float _coolDown = 3.0f;

    private GameObject _spawnedCollectible = null;
    private float _timer = 0.0f;
    private bool _collected = false;

    void Start()
    {
        Reset();
    }

    void Update()
    {
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

    public void Collect()
    {
        _collected = true;
        _spawnedCollectible = null;
    }
}
