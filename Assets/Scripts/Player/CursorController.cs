using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    [Tooltip("the speed this character will move at")]
    [SerializeField]
    private float speed;

    private Vector3 movementDirection;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(movementDirection.x + ", " + movementDirection.y);
        /*((RectTransform)*/transform.position += movementDirection * speed * Time.deltaTime;
        //Debug.Log("Transform: " + ((RectTransform)transform).position.x + ", " + ((RectTransform)transform).position.y);
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
}
