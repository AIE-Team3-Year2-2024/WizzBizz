using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSound : MonoBehaviour
{
    [Tooltip("a prefab with nothin on it other than an audio source")]
    public AudioSource audioSourcePrefab;

    [Tooltip("the sound you want played")]
    public AudioClip clip;

    [Tooltip("the maximum the pitch this can be")]
    [SerializeField]
    private float _maxPitch = 1;

    [Tooltip("the minimum the pitch this can be")]
    [SerializeField]
    private float _minPitch = 1;

    public void InstantiateSound()
    {
        AudioSource reference = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
        reference.PlayOneShot(clip);
        reference.pitch = Random.Range(_minPitch, _maxPitch);
        Destroy(reference.gameObject, clip.length);
    }
}
