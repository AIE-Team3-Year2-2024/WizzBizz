using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour
{
    [Tooltip("the minimum amount of time before the oven explodes")]
    [SerializeField]
    private float _minimumOvenTime;

    [Tooltip("the maximum amount of time before the oven explodes")]
    [SerializeField]
    private float _maximumOvenTime;

    [Tooltip("the minimum amount of time until a new fireball appears")]
    [SerializeField]
    private float _minProjectileTimer;

    [Tooltip("the maximum amount of time until a new fireball appears")]
    [SerializeField]
    private float _maxProjectileTimer;

    [Tooltip("the maximum angle varience in aiming the projectile")]
    [SerializeField]
    private float _projectileAngleVarience;

    [Tooltip("the prefab that will be created during the oven loop")]
    [SerializeField]
    private GameObject projectile;

    [Tooltip("how many projectile will be made")]
    [SerializeField]
    private int _projectileAmount;

    [Tooltip("how long the projectile will exist for")]
    [SerializeField]
    private float _lifeTime;

    [Tooltip("where the projeciles will be made")]
    [SerializeField]
    private Transform _projectileSpawn;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(OvenRoutine());
    }

    public IEnumerator OvenRoutine()
    {
        yield return new WaitForSeconds(Random.Range(_minimumOvenTime, _maximumOvenTime));
        for(int i = 0; i < _projectileAmount; i++)
        {
            yield return new WaitForSeconds(Random.Range(_minProjectileTimer, _maxProjectileTimer));
            GameObject currentProjectile = Instantiate(projectile, _projectileSpawn);
            currentProjectile.transform.rotation = Quaternion.Euler(currentProjectile.transform.rotation.eulerAngles.x, currentProjectile.transform.rotation.eulerAngles.y + (Random.Range(-_projectileAngleVarience, _projectileAngleVarience)), currentProjectile.transform.rotation.eulerAngles.z);
        }

        RestartOvenRoutine();
    }

    public void RestartOvenRoutine()
    {
        StartCoroutine(OvenRoutine());
    }
}
