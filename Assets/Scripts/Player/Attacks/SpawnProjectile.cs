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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnProjectileAtPlayer()
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

    public void FrogSpawnProjectileAtPlayer()
    {
        GameObject newProjectile = Instantiate(projectiles[Random.Range(0, projectiles.Length)], player._projectileSpawnPosition.position, player.transform.rotation);

        DamagePlayerOnCollision damage = newProjectile.GetComponent<DamagePlayerOnCollision>();
        damage.SetOwner(player);
        damage.damage *= player.damageMult;

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

    public void SpawnProjectileAtPlayer(InputAction.CallbackContext context)
    {
        if (context.started)
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

    public void FrogSpawnProjectileAtPlayer(InputAction.CallbackContext context)
    {
        if (context.started)
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
