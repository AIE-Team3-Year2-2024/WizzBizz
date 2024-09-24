using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dementia : MonoBehaviour
{
    [HideInInspector]
    public float lifeTime = 0;

    [Tooltip("the object wich contains all of the players UI")]
    [SerializeField]
    private GameObject _UIParent;

    /// <summary>
    /// manage lifetime and whther it is enabled
    /// </summary>
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            enabled = false;
        }
    }

    /// <summary>
    /// apply effect to player
    /// </summary>
    private void OnEnable()
    {
        _UIParent.SetActive(false);
    }

    /// <summary>
    /// set the player back to normal
    /// </summary>
    private void OnDisable()
    {
        _UIParent.SetActive(true);
    }
}
