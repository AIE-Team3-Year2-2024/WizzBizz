using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crippled : MonoBehaviour
{
    [Tooltip("how long this component will be enabled for")]
    [HideInInspector]
    public float lifeTime;

    private CharacterBase player;

    /// <summary>
    /// manage lifetime and whther it is enabled
    /// </summary>
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0)
        {
            enabled = false;
        }
    }

    /// <summary>
    /// apply effect to player
    /// </summary>
    private void OnEnable()
    {
        if(player == null)
        {
            player = GetComponent<CharacterBase>();
        }
        player.canDash = false;
    }

    /// <summary>
    /// set the player back to normal
    /// </summary>
    private void OnDisable()
    {
        player.canDash = true;
    }

}
