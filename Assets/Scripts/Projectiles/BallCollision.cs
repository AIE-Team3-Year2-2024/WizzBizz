using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallCollision : MonoBehaviour
{
    [Tooltip("this is the prefab of the ball object the player actually holds MUST BE SET")]
    public GameObject orbProjectilePrefab = null;

    /// <summary>
    /// if this has hit a player trigger box give them the ball and do the slow down effect on them
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        CharacterBase character = null;
        if ((character = other?.GetComponent<CharacterBase>()) != null)
        {
            if (character.hasOrb == false)
            {
                character.heldOrb = Instantiate(orbProjectilePrefab, character._projectileSpawnPosition);
                if (character.heldOrb != null)
                {
                    character.hasOrb = true;
                    character.StopCoroutine(character.CatchRoutine());
                    character.GetComponent<PlayerInput>().ActivateInput();
                    character.catchTrigger.enabled = false;
                    character.InvincibilityForFrames(5);

                    CharacterBase[] player = new CharacterBase[] { character };

                    GameManager.Instance.StartSlowDown(player);
                    Destroy(gameObject);
                }
            }
        }
    }
}
