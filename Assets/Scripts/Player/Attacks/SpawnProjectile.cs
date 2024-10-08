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
        GameObject newProjectile = Instantiate(projectiles[Random.Range(0, projectiles.Length)], player._projectileSpawnPosition.position, player.transform.rotation);

        DamagePlayerOnCollision damageComponent;
        if ((damageComponent = newProjectile.GetComponent<DamagePlayerOnCollision>()))
        {
            damageComponent.damage *= player.damageMult;
            damageComponent.SetOwner(player);
        }

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
        GameObject newProjectile = Instantiate(projectiles[Random.Range(0, projectiles.Length)], player._projectileSpawnPosition.position, player.transform.rotation);

        DamagePlayerOnCollision damageComponent;
        if ((damageComponent = newProjectile.GetComponent<DamagePlayerOnCollision>()))
        {
            damageComponent.damage *= player.damageMult;
            damageComponent.SetOwner(player);
        }

        FrogID frogid = null;
        if ((frogid = newProjectile.GetComponent<FrogID>()) != null)
        {
            frogid.ID = player.GetInstanceID();
        }

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
            GameObject newProjectile = Instantiate(projectiles[Random.Range(0, projectiles.Length)], player._projectileSpawnPosition.position, player.transform.rotation);

            DamagePlayerOnCollision damage = newProjectile.GetComponent<DamagePlayerOnCollision>();
            damage.SetOwner(player);
            damage.damage *= player.damageMult;

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
            GameObject newProjectile = Instantiate(projectiles[Random.Range(0, projectiles.Length)], player._projectileSpawnPosition.position, player.transform.rotation);

            DamagePlayerOnCollision damage = newProjectile.GetComponent<DamagePlayerOnCollision>();
            damage.SetOwner(player);
            damage.damage *= player.damageMult;

            FrogID frogid = null;
            if ((frogid = newProjectile.GetComponent<FrogID>()) != null)
            {
                frogid.ID = player.GetComponent<FrogID>().ID;
            }

            if (lifetime != 0)
            {
                Destroy(newProjectile, lifetime);
            }
        }
    }
}
