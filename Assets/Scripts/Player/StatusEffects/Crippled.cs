using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crippled : MonoBehaviour
{
    [Tooltip("how long this component will be enabled for")]
    [HideInInspector]
    public float lifeTime;

    private CharacterBase player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterBase>();
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0)
        {
            enabled = false;
        }
    }

    private void OnEnable()
    {
        if(player == null)
        {
            player = GetComponent<CharacterBase>();
        }
        player.canDash = false;
    }

    private void OnDisable()
    {
        player.canDash = true;
    }

}
