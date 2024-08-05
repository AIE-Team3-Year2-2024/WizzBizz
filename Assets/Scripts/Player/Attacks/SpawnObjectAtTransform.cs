using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectAtTransform : MonoBehaviour
{
    private CharacterBase player;

    [Tooltip("the object to be created at the players SpawnProjectile object")]
    [SerializeField]
    private GameObject projectile;

    [Tooltip("how long before this object destroys itself (wont destroy itself if set to 0)")]
    [SerializeField]
    private float lifetime;

    [Tooltip("where the projectile will be spawned")]
    [SerializeField]
    private Transform _spawn;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<CharacterBase>();
    }

    public void SpawnObjectAtSpawn()
    {
        GameObject newProjectile = Instantiate(projectile, _spawn.position, transform.rotation);

        newProjectile.GetComponent<DamagePlayerOnCollision>().SetOwner(player);

        if (lifetime != 0)
        {
            Destroy(newProjectile, lifetime);
        }
    }
}
