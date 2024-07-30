using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Tooltip("the speed this character will move at")]
    [SerializeField]
    private float speed;

    [Tooltip("the players health")]
    [SerializeField]
    private float _health;

    private Vector3 _movementDirection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _movementDirection * speed * Time.deltaTime;
    }

    /// <summary>
    /// this function is called by the players input controller wich will change the vector the charcater moves by
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        _movementDirection.z = context.ReadValue<Vector2>().y;
        _movementDirection.x = context.ReadValue<Vector2>().x;
    }


    public void OnAim(InputAction.CallbackContext context)
    {
        Vector3 aimDirection = new Vector3();
        aimDirection.z = context.ReadValue<Vector2>().y;
        aimDirection.x = context.ReadValue<Vector2>().x;
        transform.LookAt(aimDirection += transform.position, transform.up);
    }
}
