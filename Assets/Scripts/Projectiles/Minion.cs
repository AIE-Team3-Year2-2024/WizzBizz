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

    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        CharacterBase[] players = FindObjectsByType<CharacterBase>(FindObjectsSortMode.None);

        foreach (CharacterBase p in players)
        {
            targets.Add(p.transform);
        }

        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(UpdateTarget());
        
    }

    public IEnumerator UpdateTarget()
    {
        float minDist = 99999999999f;

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

        agent.destination = closestTarget.position;

        yield return new WaitForSeconds(updateTime);
        RestartTarget();
    }

    public void RestartTarget()
    {
        StartCoroutine(UpdateTarget());
    }

    public void RemoveTargetPlayer(Transform player)
    {
        targets.Remove(player);
    }
}
