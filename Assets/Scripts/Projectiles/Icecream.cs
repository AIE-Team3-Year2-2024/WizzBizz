using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icecream : MonoBehaviour
{
    [Tooltip("the amount of damge this object will deal to a player or object")]
    [SerializeField]
    private float damage;

    [Tooltip("the damage type of this attack")]
    [SerializeField]
    private CharacterBase.StaitisEffects damageEffect;

    [Tooltip("how long this attacks effect lasts for")]
    [SerializeField]
    private float effectTime;

    [Tooltip("the speed this object will move in its forwards direction")]
    [SerializeField]
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<CharacterBase>())
        {
            CharacterBase player = collision.gameObject.GetComponent<CharacterBase>();
            player.TakeDamage(damage, damageEffect, effectTime);
        }

        Destroy(gameObject);
    }
}
