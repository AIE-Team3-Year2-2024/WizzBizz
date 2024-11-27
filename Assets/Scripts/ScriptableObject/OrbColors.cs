using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "WizzBizzObjects/OrbColors")]
public class OrbColors : ScriptableObject
{
    [ColorUsage(false, false)]
    public Color innerColor;
    [ColorUsage(false, false)]
    public Color outerColor;
    [ColorUsage(false, true)]
    public Color innerGlow;

    [Range(0.0f, 1.0f)]
    public float smoothness = 1.0f;
    [Range(0.0f, 10.0f)]
    public float animationSpeed = 4.0f;
    [Range(0.0f, 0.5f)]
    public float heightScale = 0.25f;
    [Range(0.0f, 10.0f)]
    public float noise1Scale = 5.0f;
    [Range(0.0f, 25.0f)]
    public float noise2Scale = 15.0f;
}
