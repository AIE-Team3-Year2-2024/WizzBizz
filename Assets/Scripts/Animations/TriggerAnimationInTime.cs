using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimationInTime : MonoBehaviour
{
    [SerializeField, Tooltip("The name of the animation trigger to activate.")]
    private string triggerName;
    [SerializeField, Tooltip("The time that the animation will trigger at, this corresponds to the round timer.")]
    private float time = 1.0f;

    private Animator _animator;
    private bool _shouldTrigger = true;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _shouldTrigger = true;
    }

    void Update()
    {
        if (_animator == null || GameManager.Instance == null)
            return;

        if (GameManager.Instance.GetRoundTimer() <= (GameManager.Instance._roundTime - time) && _shouldTrigger == true)
        {
            _animator.SetTrigger(triggerName);
            _shouldTrigger = false;
        }
    }
}
