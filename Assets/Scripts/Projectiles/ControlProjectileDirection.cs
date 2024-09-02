using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlProjectileDirection : MonoBehaviour
{

    public void OnAim(InputAction.CallbackContext context)
    {
        Vector3 aimDirection = new Vector3();
        aimDirection.z = context.ReadValue<Vector2>().y;
        aimDirection.x = context.ReadValue<Vector2>().x;

        transform.LookAt(aimDirection += transform.position, transform.up);
    }
}
