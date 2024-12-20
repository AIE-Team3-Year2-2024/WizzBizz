using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobAttack : MonoBehaviour
{
    private CharacterBase player;

    [Tooltip("the object to be created at the players SpawnProjectile object")]
    [SerializeField]
    private GameObject projectile;

    [Tooltip("range to spawn/lob too")]
    [SerializeField]
    private float range;

    [Tooltip("the object that will show the player where there lob attack will go")]
    [SerializeField]
    private GameObject lobAimer;

    private AimChecker _checker;

    [Tooltip("how long before this object destroys itself (wont destroy itself if set to 0)")]
    [SerializeField]
    private float lifetime;

    [Tooltip("An offset to where the projectile will spawn.")]
    [SerializeField]
    [VectorRange(-100.0f, -100.0f, 0.0f, 100.0f, 100.0f, 100.0f)]
    private Vector3 spawnOffset;

    [Tooltip("the name of the trigger to be set when this attack is triggerd")]
    [SerializeField]
    private string animationTrigger;

    void Start()
    {
        player = GetComponent<CharacterBase>();

        if (lobAimer)
        {
            if (range != 0)
            {
                lobAimer.SetActive(true);
            }
            else
            {
                lobAimer.SetActive(false);
            }
            _checker = lobAimer.GetComponent<AimChecker>();
        }
    }

    /// <summary>
    /// changes the lob aimers position
    /// </summary>
    void Update()
    {
        if (lobAimer && range != 0)
        {
            lobAimer.transform.localPosition = (Vector3.forward * range * player.currentAimMagnitude) - Vector3.up;
        }
    }

    /// <summary>
    /// used to make the lobbed object (also handles lifetime)
    /// </summary>
    /// <param name="context"></param>
    public void SpawnLobObject(InputAction.CallbackContext context)
    {
        if (!_checker._colliding)
        {
            //spawn the projectile and set its end pos
            Vector3 spawnPositon = player._projectileSpawnPosition.position + spawnOffset;
            GameObject newProjectile = Instantiate(projectile, spawnPositon, player.transform.rotation);
            MoveInArc arcObject = newProjectile.GetComponent<MoveInArc>();
            arcObject.SetEndPos(lobAimer.transform.position);
            arcObject._lifeDistance = player.currentAimMagnitude * range;

            //set up projectile damage component if it has one
            DamagePlayerOnCollision damageComponent;
            if ((damageComponent = newProjectile.GetComponent<DamagePlayerOnCollision>()))
            {
                damageComponent.damage *= player.damageMult;
                damageComponent.SetOwner(player);
            }

            //set life time of projectile
            if (lifetime != 0)
            {
                Destroy(newProjectile, lifetime);
            }

            if (player.animator != null && animationTrigger != null && animationTrigger.Length > 0)
            {
                player.animator.SetTrigger(animationTrigger);
            }
        }
    }

    /// <summary>
    /// used to make the lobbed object (also handles lifetime)
    /// </summary>
    /// <param name="context"></param>
    public void SpawnLobObject()
    {
        if (!_checker._colliding)
        {
            //spawn the projectile and set its end pos
            Vector3 spawnPositon = player._projectileSpawnPosition.position + spawnOffset;
            GameObject newProjectile = Instantiate(projectile, spawnPositon, player.transform.rotation);
            MoveInArc arcObject = newProjectile.GetComponent<MoveInArc>();
            arcObject.SetEndPos(lobAimer.transform.position);
            arcObject._lifeDistance = player.currentAimMagnitude * range;

            //set up projectile damage component if it has one
            DamagePlayerOnCollision damageComponent;
            if ((damageComponent = newProjectile.GetComponent<DamagePlayerOnCollision>()))
            {
                damageComponent.damage *= player.damageMult;
                damageComponent.SetOwner(player);
            }

            //set life time of projectile
            if (lifetime != 0)
            {
                Destroy(newProjectile, lifetime);
            }

            if (player.animator != null && animationTrigger != null && animationTrigger.Length > 0)
            {
                player.animator.SetTrigger(animationTrigger);
            }
        }
    }

    /// <summary>
    /// used to make lobbed object that has a frog id on it (also handles lifetime)
    /// </summary>
    /// <param name="context"></param>
    public void FrogSpawnLobObject(InputAction.CallbackContext context)
    {
        if (!_checker._colliding)
        {
            //spawn the projectile and set its end pos
            Vector3 spawnPositon = player._projectileSpawnPosition.position + spawnOffset;
            GameObject newProjectile = Instantiate(projectile, spawnPositon, player.transform.rotation);
            MoveInArc arcObject = newProjectile.GetComponent<MoveInArc>();
            arcObject.SetEndPos(lobAimer.transform.position);
            arcObject._lifeDistance = player.currentAimMagnitude * range;

            //set up projectile damage component if it has one
            DamagePlayerOnCollision damageComponent;
            if ((damageComponent = newProjectile.GetComponent<DamagePlayerOnCollision>()) != null)
            {
                newProjectile.GetComponent<DamagePlayerOnCollision>().damage *= player.damageMult;
                damageComponent.SetOwner(player);
            }

            //set up this projectiles frog id
            FrogID frogid = null;
            if((frogid = newProjectile.GetComponent<FrogID>()) != null)
            {
                frogid.ID = player.GetComponent<FrogID>().ID;
            }

            //set up lifetime
            if (lifetime != 0)
            {
                Destroy(newProjectile, lifetime);
            }

            if (player.animator != null && animationTrigger != null && animationTrigger.Length > 0)
            {
                player.animator.SetTrigger(animationTrigger);
            }
        }
    }
}
