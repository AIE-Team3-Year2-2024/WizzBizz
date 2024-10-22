using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [Tooltip("the direction any object being hit by this will move(to controll speed just make this bigger or smaller)")]
    [SerializeField]
    private Vector3 direction;

    /// <summary>
    /// this will move any object it comes accross by the inputted vector
    /// </summary>
    /// <param name="other"></param>
    private IEnumerator OnTriggerStay(Collider other)
    {
        yield return new WaitForFixedUpdate();
        other.transform.position += direction * Time.fixedDeltaTime;
    }
}
