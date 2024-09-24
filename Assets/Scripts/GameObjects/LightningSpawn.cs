using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSpawn : MonoBehaviour
{
    [Tooltip("The minimum amount of time before lightning will spawn")]
    [SerializeField]
    private float _minTimer;

    [Tooltip("The miaximum amount of time before lightning will spawn")]
    [SerializeField]
    private float _maxTimer;

    [Tooltip("The places the lighting will spawn")]
    [SerializeField]
    private Transform[] _spawns;

    [Tooltip("the lighting prefab")]
    [SerializeField]
    private GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnLightning());
    }

    /// <summary>
    /// waits for random time and then spawns a prefab at one of the spawns randomly
    /// </summary>
    /// <returns></returns>
    public IEnumerator SpawnLightning()
    {
        yield return new WaitForSeconds(Random.Range(_minTimer, _maxTimer));
        int randPlace = Random.Range(0, _spawns.Length);
        Instantiate(prefab, _spawns[randPlace].position, Quaternion.identity);
    }

    public void RestartSpawnLightning()
    {
        StartCoroutine(SpawnLightning());
    }
}
