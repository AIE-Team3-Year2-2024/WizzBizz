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

    [Tooltip("the maximum possible volume")]
    [SerializeField]
    [Range(0, 1)]
    private float _maxVolume = 1;

    [Tooltip("the minimum possible volume")]
    [SerializeField]
    [Range(0, 1)]
    private float _minVolume = 1;

    [Tooltip("the minimum the pitch this can be")]
    [SerializeField]
    private float _minPitch = 1;

    private GameObject soundObjectReference;

    public void InstantiateSound()
    {
        if (!soundObjectReference)
        {
            AudioSource reference = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
            soundObjectReference = reference.gameObject;
            if (mixerGroup) { reference.outputAudioMixerGroup = mixerGroup; }
            reference.PlayOneShot(clip);
            reference.pitch = Random.Range(_minPitch, _maxPitch);
            reference.volume = Random.Range(_minVolume, _maxVolume);
            Destroy(reference.gameObject, clip.length);
        }
    }

    public void InstantiateSound(AudioClip clip, AudioMixerGroup mixerGroup = null, float minPitch = 1.0f, float maxPitch = 1.0f, Transform position = null, AudioSource sourcePrefab = null)
    {
        if (!soundObjectReference)
        {
            AudioSource reference = Instantiate(sourcePrefab ? sourcePrefab : audioSourcePrefab, position ? position.position : transform.position, Quaternion.identity);
            soundObjectReference = reference.gameObject;
            reference.outputAudioMixerGroup = mixerGroup ? mixerGroup : this.mixerGroup;
            reference.PlayOneShot(clip);
            reference.pitch = Random.Range(minPitch, maxPitch);
            reference.volume = Random.Range(_minVolume, _maxVolume);
            Destroy(reference.gameObject, clip.length);
        }
    }
}
