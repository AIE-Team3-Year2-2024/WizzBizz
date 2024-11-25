using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationEvents : MonoBehaviour
{
    private Animator animator;

    [SerializeField, Tooltip("the ausio source used to play sound effects from this component")]
    private AudioSource audioSource;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlaySound(string fileLoaction)
    {
        AudioClip clip = (AudioClip)Resources.Load(fileLoaction);
        audioSource.PlayOneShot(clip);
    }
}
