using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePatch : MonoBehaviour
{
    [Tooltip("How slippery the ice is.")]
    [Range(0.0f, 1.0f)]
    public float slipperyness = 1.0f;

    [Tooltip("The factor of how much to slow the character's acceleration by.")]
    [Range(0.0f, 1.0f)]
    public float accelerationFactor = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        CharacterBase character = null;
        if ((character = other?.GetComponent<CharacterBase>()) != null)
        {
            character.StartSliding(slipperyness, accelerationFactor);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CharacterBase character = null;
        if ((character = other?.GetComponent<CharacterBase>()) != null)
        {
            character.StopSliding();
        }
    }
}
