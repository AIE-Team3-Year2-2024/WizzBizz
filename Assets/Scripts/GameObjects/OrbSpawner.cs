using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _orbCollectiblePrefab;

    private GameObject _spawnedCollectible = null;

    void Start()
    {
        GameManager.Instance.orbSpawners.Add(this); // Add to the global list of orb spawners.
    }

    public void Reset()
    {
        Debug.Log("Orb now spawning!");

        if (_spawnedCollectible)
        {
            Destroy(_spawnedCollectible);
            _spawnedCollectible = null;
        }

        _spawnedCollectible = Instantiate(_orbCollectiblePrefab, transform);
        _spawnedCollectible.GetComponent<OrbCollectible>().spawner = this;
    }

    public void Collect(CharacterBase playerRef)
    {
        _spawnedCollectible = null;
        GameManager.Instance.OrbSpawnerCollected();
    }
}
