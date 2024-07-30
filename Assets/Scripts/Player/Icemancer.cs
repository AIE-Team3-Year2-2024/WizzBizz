using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Icemancer : CharacterBase
{
    [Tooltip("the icecream object that the basic attack will spawn")]
    [SerializeField]
    private GameObject icecreamPrefab;

    [Tooltip("the object that is created by the orb attack")]
    [SerializeField]
    private GameObject orbAttackPrefab;

    [Tooltip("the snowball prefab made by ability 1")]
    [SerializeField]
    private GameObject snowballPrefab;

    [Tooltip("the ice spike prefab made by ability 2")]
    [SerializeField]
    private GameObject stalagmitePrefab;


    public override void OnAttack(InputAction.CallbackContext context)
    {
        //regular attack
        if (!hasOrb)
        {
            if (context.started)
            {
                Instantiate(icecreamPrefab, projectileSpawnPosition.position, transform.rotation);
            }
        }
        else //orb attack
        {

        }
    }


    public override void OnAbility1(InputAction.CallbackContext context)
    {

    }

    public override void OnAbility2(InputAction.CallbackContext context)
    {

    }
}
