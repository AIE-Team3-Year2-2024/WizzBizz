using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [Tooltip("where the teleportee will end up")]
    [HideInInspector]
    public Transform endPoint;

    [Tooltip("the trigger used to teleport stuff")]
    public Collider trigger;

    [HideInInspector]
    public PortalAttack madeAttack;

    public int frogID;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<FrogID>()?.ID == frogID)
        {
            other.transform.position = endPoint.position;
            madeAttack.OnPortalUsed(other.gameObject);
        }
        
    }
}
