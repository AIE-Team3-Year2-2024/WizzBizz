using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectAtTransform : MonoBehaviour
{
    private CharacterBase player;

    [Tooltip("the object to be created at the players SpawnProjectile object")]
    [SerializeField]
    private GameObject projectile;

    [Tooltip("the array of objects used to spawn from in SpawnRandomAtSpawn")]
    [SerializeField]
    private GameObject[] _projectiles;

    [Tooltip("how long before this object destroys itself (wont destroy itself if set to 0)")]
    [SerializeField]
    private float lifetime;

    [Tooltip("where the projectile will be spawned")]
    [SerializeField]
    private Transform _spawn;

    // Start is called before the first frame update
    void Start()
    {
        if (player = GetComponentInParent<CharacterBase>())
        {
            return;
        }
        DamagePlayerOnCollision thisObjectDamage;
        if (thisObjectDamage = GetComponentInParent<DamagePlayerOnCollision>())
        {
            player = thisObjectDamage.GetMainOwner();
        } 
        else if (thisObjectDamage = GetComponentInParent<DamagePlayerOnCollision>())
        {
            player = thisObjectDamage.GetMainOwner();
        }
    }

    /// <summary>
    /// will spawn the projectile at _spawns position (also handles life time and damage player component settings)
    /// </summary>
    public void SpawnObjectAtSpawn()
    {
        //make this projectile at the set spawn transform
        GameObject newProjectile = Instantiate(projectile, _spawn.position, transform.rotation);

        //set up the progectiles damage component if it exists
        DamagePlayerOnCollision damageComponent;
        if ((damageComponent = newProjectile.GetComponent<DamagePlayerOnCollision>()) && player != null)
        {
            damageComponent.damage *= player.damageMult;
            damageComponent.SetOwner(player);
        }

        //set up this projectiles life time
        if (lifetime != 0)
        {
            Destroy(newProjectile, lifetime);
        }
    }


    /// <summary>
    /// will spawn a random projectile from _projectiles at _spawns position (also handles life time and damage player component settings)
    /// </summary>
    public void SpawnRandomAtSpawn()
    {
        //make this projectile at the set spawn transform
        GameObject newProjectile = Instantiate(_projectiles[Random.Range(0, _projectiles.Length)], _spawn.position, transform.rotation);

        //set up the progectiles damage component if it exists
        DamagePlayerOnCollision damageComponent;
        if ((damageComponent = newProjectile.GetComponent<DamagePlayerOnCollision>()))
        {
            damageComponent.damage *= player.damageMult;
            damageComponent.SetOwner(player);
        }

        //set up this projectiles life time
        if (lifetime != 0)
        {
            Destroy(newProjectile, lifetime);
        }
    }

    /// <summary>
    /// will spawn the projectile at _spawns position (also handles life time and damage player component settings) specifcly for projectiles with a frog id component
    /// </summary>
    public void FrogSpawnObjectAtSpawn()
    {
        //make this projectile at the set spawn transform
        GameObject newProjectile = Instantiate(projectile, _spawn.position, transform.rotation);

        //set up the progectiles damage component if it exists
        DamagePlayerOnCollision damageComponent;
        if ((damageComponent = newProjectile.GetComponent<DamagePlayerOnCollision>()) && player != null)
        {
            damageComponent.damage *= player.damageMult;
            damageComponent.SetOwner(player);
        }

        //set up the projectiles frog id
        FrogID frogid = null;
        if ((frogid = newProjectile.GetComponent<FrogID>()) != null)
        {
            frogid.ID = player.GetComponent<FrogID>().ID;
        }

        //set up this projectiles life time
        if (lifetime != 0)
        {
            Destroy(newProjectile, lifetime);
        }
    }

    /// <summary>
    /// will spawn the projectile at _spawns position (also handles life time and damage player component settings) specifcly for projectiles with a frog id component
    /// </summary>
    public void FrogSpawnRandomAtSpawn()
    {
        //make this projectile at the set spawn transform
        GameObject newProjectile = Instantiate(_projectiles[Random.Range(0, _projectiles.Length)], _spawn.position, transform.rotation);

        //set up the progectiles damage component if it exists
        DamagePlayerOnCollision damageComponent;
        if ((damageComponent = newProjectile.GetComponent<DamagePlayerOnCollision>()))
        {
            damageComponent.damage *= player.damageMult;
            damageComponent.SetOwner(player);
        }

        //set up the projectiles frog id
        FrogID frogid = null;
        if ((frogid = newProjectile.GetComponent<FrogID>()) != null)
        {
            frogid.ID = player.GetComponent<FrogID>().ID;
        }

        //set up this projectiles life time
        if (lifetime != 0)
        {
            Destroy(newProjectile, lifetime);
        }
    }
}
