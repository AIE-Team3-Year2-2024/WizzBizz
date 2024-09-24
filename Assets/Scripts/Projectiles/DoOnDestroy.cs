using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoOnDestroy : MonoBehaviour
{
    [Tooltip("any function in this event will be called when this object is destroyed")]
    public UnityEvent onDestroy;

    /// <summary>
    /// invokes the event when this object is destroyed
    /// </summary>
    public void OnDestroy()
    {
        onDestroy.Invoke();
    }
}
