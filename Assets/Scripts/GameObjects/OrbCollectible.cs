using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbCollectible : MonoBehaviour
{
    [HideInInspector] public OrbSpawner spawner = null;

    public GameObject orbProjectilePrefab = null;

    public Transform orbModel = null;

    [SerializeField] private float _scaleTransitionSeconds = 1.0f;

    private bool _justSpawned = false;
    private float _scaleTransitionTimer = 0.0f;
    private Vector3 _scaleTransition = Vector3.zero;

    void Awake()
    {
        _justSpawned = false;
        _scaleTransition = Vector3.zero;
        _scaleTransitionTimer = 0.0f;

        if (orbModel != null)
        {
            orbModel.localScale = _scaleTransition;
            _justSpawned = true;
        }
    }

    /// <summary>
    /// handles the balls spawing animation
    /// </summary>
    void Update()
    {
        if (_justSpawned == true)
        {
            _scaleTransitionTimer += Time.deltaTime;
            float normalizedTimer = _scaleTransitionTimer / _scaleTransitionSeconds;
            float t = normalizedTimer < 0.5f ? 4.0f * normalizedTimer * normalizedTimer * normalizedTimer : 1.0f - Mathf.Pow(-2.0f * normalizedTimer + 2.0f, 3.0f) / 2.0f; // https://easings.net/#easeInOutCubic
            _scaleTransition = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            if (_scaleTransitionTimer >= _scaleTransitionSeconds)
            {
                _scaleTransitionTimer = _scaleTransitionSeconds;
                _justSpawned = false;
            }
            orbModel.localScale = _scaleTransition;
        }
    }

    /// <summary>
    /// if hit a player give the player an orb 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        CharacterBase character = null;
        if ((character = other?.GetComponent<CharacterBase>()) != null)
        {
            if (character.hasOrb == false)
            {
                character.heldOrb = Instantiate(orbProjectilePrefab, character._projectileSpawnPosition);
                if (character.heldOrb != null) 
                {
                    character.hasOrb = true;
                    spawner.Collect(character);
                    character.StartKillBall(character.heldOrb);
                    Destroy(gameObject);
                }
            }
        }
    }
}
