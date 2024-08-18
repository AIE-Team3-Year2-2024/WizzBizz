using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        Destroy(gameObject);
    }
}
