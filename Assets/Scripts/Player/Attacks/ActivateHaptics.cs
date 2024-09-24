using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateHaptics : MonoBehaviour
{
    [Tooltip("what the low frequency motor will bet set to")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float lowFreq;

    [Tooltip("what the high frequency motor will bet set to")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float highFreq;

    private CharacterBase player;

    [Tooltip("how long this vibration will last")]
    [SerializeField]
    private float time;
    
    /// <summary>
    /// find player
    /// </summary>
    void Start()
    {
        player = GetComponent<CharacterBase>();
    }

    /// <summary>
    /// tells the player to vibrate thier controller
    /// </summary>
    public void Vibrate()
    {
        StartCoroutine(player.AdjustableHaptics(lowFreq, highFreq, time));
    }
}
