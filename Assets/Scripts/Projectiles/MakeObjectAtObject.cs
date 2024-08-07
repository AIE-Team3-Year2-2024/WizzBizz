using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeObjectAtObject : MonoBehaviour
{
    [Tooltip("the object to be made")]
    [SerializeField]
    private GameObject _prefab;

    [Tooltip("the object where _prefab will be made (ifleft blanck defaults to the object with this component on it)")]
    [SerializeField]
    private GameObject _spawn;

    public void MakeObject()
    {
        if(_spawn != null)
        {
            Instantiate(_prefab, _spawn.transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_prefab, transform.position, Quaternion.identity);
        }
    }
}
