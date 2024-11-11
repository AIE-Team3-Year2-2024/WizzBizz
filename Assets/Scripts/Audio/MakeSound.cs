using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSound : MonoBehaviour
{
    [Tooltip("a prefab with nothin on it other than an audio source")]
    public AudioSource audioSourcePrefab;

    [Tooltip("the sound you want played")]
    public AudioClip clip;

    public void InstantiateSound()
    {
        AudioSource reference = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
        reference.PlayOneShot(clip);
        Destroy(reference.gameObject, clip.length);
    }
}
