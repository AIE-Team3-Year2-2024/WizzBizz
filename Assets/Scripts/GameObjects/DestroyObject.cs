using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField, Tooltip("the object to Destroy")]
    private GameObject _objectToDestroy;
    
    public void DoDestroy()
    {
        Destroy(_objectToDestroy);
    }
}
