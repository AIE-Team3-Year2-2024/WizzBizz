using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Minion : MonoBehaviour
{
    [Tooltip("the transforms of the other players to target")]
    private List<Transform> targets = new List<Transform>();
    
    [Tooltip("how often to update this agents target")]
    [SerializeField]
    private float updateTime;

    private Transform ownerTransform;

    private NavMeshAgent agent;

    /// <summary>
    /// finds all players in scene other than the owner and starts the coroutine loop
    /// </summary>
    void Start()
    {
        CharacterBase[] players = FindObjectsByType<CharacterBase>(FindObjectsSortMode.None);

        foreach (CharacterBase p in players)
        {
            if (p.transform != ownerTransform)
            {
                targets.Add(p.transform);
            }
        }

        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(UpdateTarget());
        
    }

    /// <summary>
    /// will find the closest transform in targets and set that as the destination for this objects nav mesh agent
    /// </summary>
    /// <returns></returns>
    public IEnumerator UpdateTarget()
    {
        yield return new WaitForEndOfFrame();

        float minDist = float.PositiveInfinity;

        Transform closestTarget = null;
        foreach (Transform t in targets)
        {
            float dist = Vector3.Distance(t.position, transform.position);
            if (dist < minDist)
            {
                closestTarget = t;
                minDist = dist;
            }
        }

        if (closestTarget == null)
        {
            agent.destination = Vector3.zero;
        }
        else
        {
            agent.destination = closestTarget.position;
        }

        yield return new WaitForSeconds(updateTime);
        RestartTarget();
    }

    /// <summary>
    /// restarts the coroutine for finding a player to target
    /// </summary>
    public void RestartTarget()
    {
        StartCoroutine(UpdateTarget());
    }

    /// <summary>
    /// sets the inputted transform as the minions owner and removes it from the list players 
    /// </summary>
    /// <param name="player"></param>
    public void RemoveTargetPlayer(Transform player)
    {
        targets.Remove(player);
        ownerTransform = player;
    }
}
