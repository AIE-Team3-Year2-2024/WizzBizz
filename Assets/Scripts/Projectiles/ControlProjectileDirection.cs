using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlProjectileDirection : MonoBehaviour
{
    /// <summary>
    /// will rotate this object based on the vector in context
    /// </summary>
    /// <param name="context"></param>
    public void OnAim(InputAction.CallbackContext context)
    {
        Vector3 aimDirection = new Vector3();
        aimDirection.z = context.ReadValue<Vector2>().y;
        aimDirection.x = context.ReadValue<Vector2>().x;

        transform.LookAt(aimDirection += transform.position, transform.up);
    }
}
