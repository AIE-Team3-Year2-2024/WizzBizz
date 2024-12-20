using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnObjectAtAim : MonoBehaviour
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

    [Tooltip("the name of the trigger to be set when this attack is triggerd")]
    [SerializeField]
    private string animationTrigger;

    // Start is called before the first frame update
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
            lobAimer.transform.localPosition = Vector3.forward * range * player.currentAimMagnitude;
        }
    }

    /// <summary>
    /// used to make the object at the lob aimers position (alos handles life time damage player component seetings and minion owning)
    /// </summary>
    /// <param name="context"></param>
    public void SpawnObjectAtAimFunction(InputAction.CallbackContext context)
    {
        if (!_checker._colliding)
        {
            //make this projectile at this attacks aimer
            GameObject newProjectile = Instantiate(projectile, lobAimer.transform.position, player.transform.rotation);

            //set up the projectiles damage component if it exists
            DamagePlayerOnCollision damageComponent;
            if ((damageComponent = newProjectile.GetComponent<DamagePlayerOnCollision>()))
            {
                damageComponent.damage *= player.damageMult;
                damageComponent.SetOwner(player);
            }

            //set up the minion components of the projectile if it has minion components
            Minion[] minion;
            if ((minion = newProjectile.GetComponentsInChildren<Minion>()) != null)
            {
                foreach(Minion minio in minion)
                {
                    minio.RemoveTargetPlayer(player.transform);
                }
            }

            //set up projectile life time
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
    /// used to make an object with a frog id at the lobaimers position (alos handles life time damage player component seetings and minion owning)
    /// </summary>
    /// <param name="context"></param>
    public void FrogSpawnObjectAtAimFunction(InputAction.CallbackContext context)
    {
        if (!_checker._colliding)
        {
            //make this projectile at this attacks aimer
            GameObject newProjectile = Instantiate(projectile, lobAimer.transform.position, player.transform.rotation);

            //set up the projectiles damage component if it exists
            DamagePlayerOnCollision damageComponent;
            if ((damageComponent = newProjectile.GetComponent<DamagePlayerOnCollision>()) != null)
            {
                damageComponent.damage *= player.damageMult;
                damageComponent.SetOwner(player);
            }

            //set up the projectiles damage component if it exists
            Minion[] minion;
            if ((minion = newProjectile.GetComponentsInChildren<Minion>()) != null)
            {
                foreach (Minion minio in minion)
                {
                    minio.RemoveTargetPlayer(player.transform);
                }
            }

            //set up this projectiles frog id
            FrogID frogid = null;
            if ((frogid = newProjectile.GetComponent<FrogID>()) != null)
            {
                frogid.ID = player.GetComponent<FrogID>().ID;
            }

            //set up projectile life time
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
