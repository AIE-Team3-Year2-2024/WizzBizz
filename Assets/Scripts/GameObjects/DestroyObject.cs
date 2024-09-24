using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField, Tooltip("the object to destroy")]
    private GameObject _objectToDestroy;
    
    /// <summary>
    /// calls destroy on the object this is storing
    /// </summary>
    public void DoDestroy()
    {
        Destroy(_objectToDestroy);
    }
}
