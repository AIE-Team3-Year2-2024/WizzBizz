using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnProjectile : MonoBehaviour
{
    private CharacterBase player;

    [Tooltip("the object to be created at the players SpawnProjectile object")]
    [SerializeField]
    private GameObject[] projectiles;

    [Tooltip("how long before this object destroys itself (wont destroy itself if set to 0)")]
    [SerializeField]
    private float lifetime;

    [Tooltip("An offset to where the projectile will spawn.")]
    [SerializeField]
    [VectorRange(-100.0f, -100.0f, 0.0f, 100.0f, 100.0f, 100.0f)]
    private Vector3 spawnOffset;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterBase>();
    }

    /// <summary>
    /// will spawn one of the objects from the projectiles array randomly at the player projectile spawn (also handles life time and damage player settings)
    /// </summary>
    public void SpawnProjectileAtPlayer()
    {
        //spawns one of the projectiles at this players projectile spawn transform
        Vector3 spawnPositon = player._projectileSpawnPosition.position + spawnOffset;
        GameObject newProjectile = Instantiate(projectiles[Random.Range(0, projectiles.Length)], spawnPositon, player.transform.rotation);

        //sets up the projectiles damage component if it has it
        DamagePlayerOnCollision damageComponent;
        if ((damageComponent = newProjectile.GetComponent<DamagePlayerOnCollision>()))
        {
            damageComponent.damage *= player.damageMult;
            damageComponent.SetOwner(player);
        }

        //sets up life time
        if (lifetime != 0)
        {
            Destroy(newProjectile, lifetime);
        }
    }

    /// <summary>
    /// will spawn one of the objects that have a frog id from the projectiles array randomly at the player projectile spawn (also handles life time and damage player settings)
    /// </summary>
    public void FrogSpawnProjectileAtPlayer()
    {
        //spawns one of the projectiles at this players projectile spawn transform
        Vector3 spawnPositon = player._projectileSpawnPosition.position + spawnOffset;
        GameObject newProjectile = Instantiate(projectiles[Random.Range(0, projectiles.Length)], spawnPositon, player.transform.rotation);

        //sets up the projectiles damage component if it has it
        DamagePlayerOnCollision damageComponent;
        if ((damageComponent = newProjectile.GetComponent<DamagePlayerOnCollision>()))
        {
            damageComponent.damage *= player.damageMult;
            damageComponent.SetOwner(player);
        }

        //sets up this projectiles frog id
        FrogID frogid = null;
        if ((frogid = newProjectile.GetComponent<FrogID>()) != null)
        {
            frogid.ID = player.GetInstanceID();
        }

        ////sets up life time
        if (lifetime != 0)
        {
            Destroy(newProjectile, lifetime);
        }
    }

    /// <summary>
    /// will spawn one of the objects from the projectiles array randomly at the player projectile spawn (also handles life time and damage player settings)
    /// </summary>
    public void SpawnProjectileAtPlayer(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //spawns one of the projectiles at this players projectile spawn transform
            Vector3 spawnPositon = player._projectileSpawnPosition.position + spawnOffset;
            GameObject newProjectile = Instantiate(projectiles[Random.Range(0, projectiles.Length)], spawnPositon, player.transform.rotation);

            //sets up the projectiles damage component if it has it
            DamagePlayerOnCollision damageComponent;
            if ((damageComponent = newProjectile.GetComponent<DamagePlayerOnCollision>()))
            {
                damageComponent.damage *= player.damageMult;
                damageComponent.SetOwner(player);
            }

            ////sets up life time
            if (lifetime != 0)
            {
                Destroy(newProjectile, lifetime);
            }
        }
    }

    /// <summary>
    /// will spawn one of the objects that have a frog id from the projectiles array randomly at the player projectile spawn (also handles life time and damage player settings)
    /// </summary>
    public void FrogSpawnProjectileAtPlayer(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //spawns one of the projectiles at this players projectile spawn transform
            Vector3 spawnPositon = player._projectileSpawnPosition.position + spawnOffset;
            GameObject newProjectile = Instantiate(projectiles[Random.Range(0, projectiles.Length)], spawnPositon, player.transform.rotation);

            //sets up the projectiles damage component if it has it
            DamagePlayerOnCollision damageComponent;
            if ((damageComponent = newProjectile.GetComponent<DamagePlayerOnCollision>()))
            {
                damageComponent.damage *= player.damageMult;
                damageComponent.SetOwner(player);
            }

            //sets up this projectiles frog id
            FrogID frogid = null;
            if ((frogid = newProjectile.GetComponent<FrogID>()) != null)
            {
                frogid.ID = player.GetComponent<FrogID>().ID;
            }

            //sets up life time
            if (lifetime != 0)
            {
                Destroy(newProjectile, lifetime);
            }
        }
    }
}
