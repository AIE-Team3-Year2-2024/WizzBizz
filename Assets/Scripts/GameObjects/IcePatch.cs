using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class IcePatch : MonoBehaviour
{
    [Tooltip("How slippery the ice is.")]
    [Range(0.0f, 1.0f)]
    public float slipperyness = 1.0f;

    [Tooltip("The factor of how much to slow the character's acceleration by.")]
    [Range(0.0f, 1.0f)]
    [HideInInspector] public float accelerationFactor = 0.5f; // Unused.

    private List<CharacterBase> _charactersInside = new List<CharacterBase>();

    /// <summary>
    /// make any player touching this start sliding
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        CharacterBase character = null;
        if ((character = other?.GetComponent<CharacterBase>()) != null)
        {
            character.StartSliding(slipperyness, accelerationFactor);
            _charactersInside.Add(character);
        }
    }

    /// <summary>
    /// make any player who is no longer touching this stop sliding
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        CharacterBase character = null;
        if ((character = other?.GetComponent<CharacterBase>()) != null)
        {
            character.StopSliding();
            _charactersInside.Remove(character);
        }
    }

    /// <summary>
    /// make sure any charcaters currently affected by this are set to normal
    /// </summary>
    private void OnDestroy()
    {
        if (_charactersInside.Count > 0)
        {
            foreach (CharacterBase c in _charactersInside)
            {
                c.StopSliding();
            }
        }
    }
}
