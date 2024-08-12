using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallCollision : MonoBehaviour
{

    public GameObject orbProjectilePrefab = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
                    Destroy(gameObject);
                }
            }
        }
    }
}
