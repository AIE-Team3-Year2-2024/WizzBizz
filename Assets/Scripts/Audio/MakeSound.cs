using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MakeSound : MonoBehaviour
{
    [Tooltip("a prefab with nothin on it other than an audio source")]
    public AudioSource audioSourcePrefab;

    [Tooltip("the sound you want played")]
    public AudioClip clip;

    [Tooltip("which mixing channel to use")]
    public AudioMixerGroup mixerGroup;

    [Tooltip("the maximum the pitch this can be")]
    [SerializeField]
    private float _maxPitch = 1;

    [Tooltip("the minimum the pitch this can be")]
    [SerializeField]
    private float _minPitch = 1;

    public void InstantiateSound()
    {
        AudioSource reference = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
        if (mixerGroup) { reference.outputAudioMixerGroup = mixerGroup; }
        reference.PlayOneShot(clip);
        reference.pitch = Random.Range(_minPitch, _maxPitch);
        Destroy(reference.gameObject, clip.length);
    }

    public void InstantiateSound(AudioClip clip, AudioMixerGroup mixerGroup = null, float minPitch = 1.0f, float maxPitch = 1.0f, Transform position = null, AudioSource sourcePrefab = null)
    {
        AudioSource reference = Instantiate(sourcePrefab ? sourcePrefab : audioSourcePrefab, position ? position.position : transform.position, Quaternion.identity);
        reference.outputAudioMixerGroup = mixerGroup ? mixerGroup : this.mixerGroup;
        reference.PlayOneShot(clip);
        reference.pitch = Random.Range(minPitch, maxPitch);
        Destroy(reference.gameObject, clip.length);
    }
}
