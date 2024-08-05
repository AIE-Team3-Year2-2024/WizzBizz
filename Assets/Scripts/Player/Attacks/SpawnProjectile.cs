using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnProjectile : MonoBehaviour
{
    private CharacterBase player;

    [Tooltip("the object to be created at the players SpawnProjectile object")]
    [SerializeField]
    private GameObject projectile;

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
        GameObject newProjectile = Instantiate(projectile, player.projectileSpawnPosition.position, player.transform.rotation);

        newProjectile.GetComponent<DamagePlayerOnCollision>().SetOwner(player);

        if (lifetime != 0)
        {
            Destroy(newProjectile, lifetime);
        }
    }
}
