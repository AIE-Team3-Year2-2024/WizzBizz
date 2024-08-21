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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0 )
        {
            enabled = false;
        }
    }

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

    private void OnDisable()
    {
        input.ActivateInput();
    }
}
