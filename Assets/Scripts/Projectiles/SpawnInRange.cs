using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnInRange : MonoBehaviour
{
    [Tooltip("how far away the prefab can be spawned")]
    [SerializeField]
    private float _range;

    [Tooltip("the minimum amount of time before spawning another prefab")]
    [SerializeField]
    private float _minSpawnTime;

    [Tooltip("the maximum amount of time before spawning another prefab")]
    [SerializeField]
    private float _maxSpawnTime;

    [Tooltip("the amount of objects to be made")]
    [SerializeField]
    private int _objectAmount;

    [Tooltip("the object to be spawned in the area")]
    [SerializeField]
    private GameObject _object;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    /// <summary>
    /// will spawn object on a loop with its position randomly being within range of this objects position
    /// </summary>
    /// <returns></returns>
    public IEnumerator SpawnLoop()
    {
        for(int i = 0; i < _objectAmount; i++)
        {
            yield return new WaitForSeconds(Random.Range(_minSpawnTime, _maxSpawnTime));
            Instantiate(_object, new Vector3(transform.position.x + (Random.Range(-_range, _range)), transform.position.y, transform.position.z + (Random.Range(-_range, _range))), Quaternion.identity);
        }
    }
}
