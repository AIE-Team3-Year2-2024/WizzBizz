using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    [Tooltip("the speed this character will move at")]
    [SerializeField]
    private float speed;

    private ColliderButton lastCollidedButton;

    private Vector3 movementDirection;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    { 
        transform.position += movementDirection * speed * Time.deltaTime;
        
    }

    /// <summary>
    /// this function is called by the players input controller wich will change the vector the charcater moves by
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        movementDirection.y = context.ReadValue<Vector2>().y;
        movementDirection.x = context.ReadValue<Vector2>().x;
    }


    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<ColliderButton>())
        {
            lastCollidedButton = collision.gameObject.GetComponent<ColliderButton>();
        }
    }

    public void OnAccept()
    {
        lastCollidedButton.Pressed();
    }
}
