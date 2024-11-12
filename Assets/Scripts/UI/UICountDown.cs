using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UICountDown : MonoBehaviour
{
    [Tooltip("List of UI objects for the count down.")]
    public List<UICount> counts = new List<UICount>();

    [HideInInspector]
    public event Action countdownEnd = null; // Callback for when the countdown has finished. 

    private float _time = 3.0f; // Max time.
    private float _clock = 0.0f; // The current time.
    private bool _startCounting = false; // Should it start counting down?
    private int _counted = 0; // The objects that have already been counted.

    private void Start()
    {
        _time = counts.Count; // Max time should be the same as the amount of count down objects.

        foreach (UICount c in counts) // Setup count down objects.
        {
            c.gameObject.SetActive(true);
            c.Awake(); // Make sure it gets it's components.
            c.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (_startCounting)
        {
            // Count down finish.
            if (_clock <= 0.0f)
            {
                _clock = 0.0f; // Reset.
                _startCounting = false;
                if (countdownEnd != null)
                    countdownEnd.Invoke(); // Callback.
                return;
            }

            // Enable count down objects in time.
            for (int i = _counted; i > 0; --i)
            {
                if (_clock <= i)
                {
                    counts[i-1].gameObject.SetActive(true);
                    _counted--;
                }
            }

            _clock -= Time.unscaledDeltaTime; // Count down in seconds.
        }
    }

    public void StartCountDown()
    {
        // Setup count down start.
        _clock = _time;
        _counted = counts.Count;
        _startCounting = true;
    }

    public void StopCountDown()
    {
        // Reset everything.
        _clock = 0.0f;
        _counted = 0;
        _startCounting = false;

        foreach (UICount c in counts)
        {
            c.gameObject.SetActive(false);
        }
    }

}
