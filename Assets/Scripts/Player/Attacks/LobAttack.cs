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

    // Update is called once per frame
    void Update()
    {
        if (lobAimer && range != 0)
        {
            lobAimer.transform.localPosition = (Vector3.forward * range * player.currentAimMagnitude) - Vector3.up;
        }
    }

    public void SpawnLobObject(InputAction.CallbackContext context)
    {
        if (_checker.currentCollisions < 1)
        {
            GameObject newProjectile = Instantiate(projectile, player._projectileSpawnPosition.position, player.transform.rotation);
            newProjectile.GetComponent<MoveInArc>().SetEndPos(lobAimer.transform.position);

            if(newProjectile.GetComponent<DamagePlayerOnCollision>())
            {
                newProjectile.GetComponent<DamagePlayerOnCollision>().damage *= player.damageMult;
            }

            if (lifetime != 0)
            {
                Destroy(newProjectile, lifetime);
            }
        }
    }

    public void FrogSpawnLobObject(InputAction.CallbackContext context)
    {
        if (_checker.currentCollisions < 1)
        {

            GameObject newProjectile = Instantiate(projectile, player._projectileSpawnPosition.position, player.transform.rotation);
            newProjectile.GetComponent<MoveInArc>().SetEndPos(lobAimer.transform.position);

            DamagePlayerOnCollision damageComponent;
            if ((damageComponent = newProjectile.GetComponent<DamagePlayerOnCollision>()) != null)
            {
                newProjectile.GetComponent<DamagePlayerOnCollision>().damage *= player.damageMult;
            }

            FrogID frogid = null;
            if((frogid = newProjectile.GetComponent<FrogID>()) != null)
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
