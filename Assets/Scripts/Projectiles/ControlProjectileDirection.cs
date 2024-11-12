using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlProjectileDirection : MonoBehaviour
{
    [SerializeField, Tooltip("the speed at wich this will rotate to the inputted direction")]
    private float rotationSpeed;

    private Vector3 aimDirection;

    private void Start()
    {
        aimDirection = transform.forward;
    }

    /// <summary>
    /// will rotate this object based on the vector in context
    /// </summary>
    /// <param name="context"></param>
    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>() != Vector2.zero)
        {
            aimDirection.z = context.ReadValue<Vector2>().y;
            aimDirection.x = context.ReadValue<Vector2>().x;
        }

        //transform.LookAt(aimDirection += transform.position, transform.up);

        //float angle = Mathf.Atan2(aimDirection.y, aimDirection.x);
        //float degrees = 180 * angle/Mathf.PI;

        aimDirection.Normalize();
    }

    private void Update()
    {
        Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(aimDirection), rotationSpeed * Time.deltaTime);
        if(aimDirection == Vector3.zero)
        {
            Debug.LogWarning("aim direction on the projectile control component is 0 this means the player to control is likely not set check this component is in the onjebts damage player component");
        }

        rotation.z = 0;
        rotation.x = 0;

        transform.rotation = rotation;
    }
}
