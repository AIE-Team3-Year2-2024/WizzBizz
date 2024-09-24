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

    /// <summary>
    /// will spawn the prefab at spawns position if spawn exists otherwise it will spawn it at this objects position
    /// </summary>
    /// <param name="context"></param>
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

    /// <summary>
    /// will spawn the prefab at spawns position if spawn exists otherwise it will spawn it at this objects position
    /// </summary>
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

    /// <summary>
    /// will spawn the prefab as a child of this object 
    /// </summary>
    public void MakeObjectAsChild()
    {
        Instantiate(_prefab, transform);
    }

    /// <summary>
    /// will spawn the prefab as a child of this object 
    /// </summary>
    /// <param name="context"></param>
    public void MakeObjectAsChild(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Instantiate(_prefab, transform);
        }
    }
}
