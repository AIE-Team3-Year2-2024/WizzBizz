using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    [Tooltip("the speed this character will move at")]
    [SerializeField]
    private float _speed;

    private ReadyButton _lastCollidedReadyButton;

    private CharacterButton _lastCollidedCharacterButton;

    public bool canMove = true;

    private bool playerSelected = false;

    private Vector3 _movementDirection;

    public int playerID;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    { 
        transform.position += _movementDirection * _speed * Time.deltaTime;
        
    }

    /// <summary>
    /// this function is called by the players input controller wich will change the vector the charcater moves by
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            _movementDirection.y = context.ReadValue<Vector2>().y;
            _movementDirection.x = context.ReadValue<Vector2>().x;
        }
    }

    /// <summary>
    /// keeps track of the current button
    /// </summary>
    /// <param name="collision"></param>
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<ReadyButton>() != null)
        {
            _lastCollidedReadyButton = collision.gameObject.GetComponent<ReadyButton>();
        } 
        else if(collision.gameObject.GetComponent<CharacterButton>() != null)
        {
            _lastCollidedCharacterButton = collision.gameObject.GetComponent<CharacterButton>();
        }
    }

    /// <summary>
    /// keeps track of the current button
    /// </summary>
    /// <param name="collision"></param>
    public void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.GetComponent<ReadyButton>() == _lastCollidedReadyButton)
        {
            _lastCollidedReadyButton = null;
        } 
        else if (collision.gameObject.GetComponent<CharacterButton>() == _lastCollidedCharacterButton)
        {
            _lastCollidedCharacterButton = null;
        }
    }

    /// <summary>
    /// will press the last button
    /// </summary>
    /// <param name="context"></param>
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (_lastCollidedReadyButton != null && context.performed && playerSelected)
        {
            _lastCollidedReadyButton.PlayerInteract(this);
            _movementDirection = Vector3.zero;
        }
        if(_lastCollidedCharacterButton != null && context.performed)
        {
            GameManager.Instance.SetSelectedCharacter(playerID, _lastCollidedCharacterButton.character);
            playerSelected = true;
        }
    }

    /// <summary>
    /// tell the game manager this player has disconnected and destroys this object
    /// </summary>
    public void OnDisconnect()
    {
        GameManager.Instance.DissconectCursor(GetComponent<PlayerInput>());
        Destroy(gameObject);
    }
}
