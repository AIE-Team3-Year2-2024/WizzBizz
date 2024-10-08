using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Stun : MonoBehaviour
{
    [HideInInspector]
    public float lifeTime;

    private PlayerInput input;
    private CharacterBase player;

    /// <summary>
    /// manage lifetime and whther it is enabled
    /// </summary>
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0 )
        {
            enabled = false;
        }
    }

    /// <summary>
    /// apply effect to player
    /// </summary>
    private void OnEnable()
    {
        if(input == null)
        {
            input = GetComponent<PlayerInput>();
            player = GetComponent<CharacterBase>();
        }

        input.DeactivateInput();
        player.CancelPlayerMovement();
    }


    /// <summary>
    /// set the player back to normal
    /// </summary>s
    private void OnDisable()
    {
        input.ActivateInput();
    }
}
