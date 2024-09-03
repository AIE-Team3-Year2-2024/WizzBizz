using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MakeObjectAtObject : MonoBehaviour
{
    [Tooltip("the object to be made")]
    [SerializeField]
    private GameObject _prefab;

    [Tooltip("the object where _prefab will be made (i left blank defaults to the object with this component on it)")]
    [SerializeField]
    private GameObject _spawn;

    public void MakeObject(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_spawn != null)
            {
                Instantiate(_prefab, _spawn.transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_prefab, transform.position, Quaternion.identity);
            }
        }
    }

    public void MakeObject()
    {
        if (_spawn != null)
        {
            Instantiate(_prefab, _spawn.transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_prefab, transform.position, Quaternion.identity);
        }
    }

    public void MakeObjectAsChild()
    {
        Instantiate(_prefab, transform);
    }

    public void MakeObjectAsChild(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Instantiate(_prefab, transform);
        }
    }
}
