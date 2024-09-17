using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Tooltip("where the teleportee will end up")]
    [SerializeField]
    private Teleporter[] endPoints;

    private Collider teleCollider;

    [Tooltip("how long this will stay off for after it is enterd or exited")]
    [SerializeField]
    private float _offTime;

    private void Start()
    {
        teleCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Teleporter otherTele = endPoints[Random.Range(0, endPoints.Length)];
        other.transform.position = otherTele.transform.position;
        otherTele.StartOffRoutine();
        StartOffRoutine();
    }

    public void StartOffRoutine()
    {
        StartCoroutine(OffRoutine());
    }

    public IEnumerator OffRoutine()
    {
        teleCollider.enabled = false;

        yield return new WaitForSeconds(_offTime);

        teleCollider.enabled = true;
    }
}
